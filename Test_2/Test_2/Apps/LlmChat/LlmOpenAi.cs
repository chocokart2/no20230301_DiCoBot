using OpenAI.Assistants;
using OpenAI.Files;
using OpenAI.VectorStores;
using System;
using System.ClientModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Test_2.Base;
using Test_2.Code.Object;

#pragma warning disable OPENAI001

namespace Test_2.Apps.LlmChat
{
    internal class LlmOpenAi : IMessageToMethod
    {
        private static bool _initialized = false;

        private static AssistantClient assistantClient;
        private static OpenAIFileClient fileClient;
        private static Assistant assistant;
        private static string vectorStoreId;

        /// <summary>
        /// 최초 1회만 파일 업로드 + 벡터스토어 생성 + Assistant 생성
        /// </summary>
        private async Task<MessageToMethodResult> InitializeAsync()
        {
            MessageToMethodResult result = new MessageToMethodResult()
            {
                isSuccess = false,
                isPrintMessage = true,
                message = "초기화 실패",
                printMessage = "초기화 실패"
            };

            if (_initialized)
            {
                return new MessageToMethodResult()
                {
                    isSuccess = true,
                    isPrintMessage = true,
                    message = ">> LlmOpenAi.InitializeAsync() 이미 초기화됨",
                    printMessage = ">> LlmOpenAi.InitializeAsync() 이미 초기화됨"
                };
            }


            var client = Singleton.instance.openAIClient;
            assistantClient = client.GetAssistantClient();
            fileClient = client.GetOpenAIFileClient();

            // 1) 상대 경로 파일 읽기
            //string relative = "./myfile.md";
            //string full = Path.GetFullPath(relative);

            string baseDir = AppContext.BaseDirectory;
            string full = Path.Combine(baseDir, "../../../../../DiCoBot/myfile.md");
            if (!File.Exists(full))
                throw new FileNotFoundException($"지정된 상대 경로 파일을 찾을 수 없습니다: {full}");

            // 2) 업로드
            using var fs = File.OpenRead(full);
            ClientResult<OpenAIFile> uploadedFile = fileClient.UploadFile(
                fs,
                Path.GetFileName(full),
                FileUploadPurpose.Assistants);

            // 3) 벡터스토어 생성
            var vsClient = Singleton.instance.openAIClient.GetVectorStoreClient();

            var vsOptions = new VectorStoreCreationOptions();
            vsOptions.FileIds.Add(uploadedFile.Value.Id);

            VectorStore store = await vsClient.CreateVectorStoreAsync(vsOptions);
            store = await PollVectorStoreAsync(vsClient, store.Id);

            vectorStoreId = store.Id;

            // 4) Assistant 생성
            var assistantOptions = new AssistantCreationOptions()
            {
                Name = "Local RAG Assistant",
                //Instructions =
                //    "You MUST answer strictly based on the contents of the uploaded file 'myfile.md'. " +
                //    "If the answer is not found in the file, say '파일에 해당 정보가 없습니다.'"
                Instructions =
                      "당신은 게임 기획서가 적힌 노션 JSON 파일을 읽고 이해하는 어시스턴트입니다."
                      + " 사용자가 질문하면, 반드시 해당 노션 마크다운 파일의 내용에 기반해서 한국어 두루높임 비격식체로 답변해야 합니다."
                      + " 만약 질문과 관련된 내용이 JSON 파일에 없으면, \"해당 내용이 마크다운 파일에 없습니다\"라고 답변해야 합니다."
                      + " 사용자가 개선사항을 요구한다면, 필요하다면 참고용 일반적인 안내를 덧붙일 수 있습니다.",
            };

            assistantOptions.Tools.Add(new FileSearchToolDefinition());
            assistantOptions.ToolResources = new ToolResources()
            //assistantOptions.ToolResources = new AssistantToolResources()
            {
                FileSearch = new FileSearchToolResources()
                {
                    VectorStoreIds = { vectorStoreId }
                }
            };

            assistant = assistantClient.CreateAssistant("gpt-4o", assistantOptions);

            _initialized = true;

            return new MessageToMethodResult()
            {
                isSuccess = true,
                isPrintMessage = true,
                message = ">> LlmOpenAi.InitializeAsync() 초기화 성공",
                printMessage = ">> LlmOpenAi.InitializeAsync() 초기화 성공"
            };
        }


        /// <summary>
        /// 실제 사용자 질의(message)를 받아 RAG 기반 답변 생성
        /// </summary>
        public async Task<MessageToMethodResult> AssignWorkAsync(string message)
        {
            // 초기화 수행
            await InitializeAsync();

            // Thread 생성 및 실행
            var threadOptions = new ThreadCreationOptions()
            {
                InitialMessages = { message }
            };

            ThreadRun run = assistantClient.CreateThreadAndRun(assistant.Id, threadOptions);

            // 완료될 때까지 폴링
            do
            {
                Thread.Sleep(800);
                run = assistantClient.GetRun(run.ThreadId, run.Id);
            }
            while (!run.Status.IsTerminal);

            // 메시지 가져오기
            var msgs = assistantClient.GetMessages(
                run.ThreadId,
                new MessageCollectionOptions() { Order = MessageCollectionOrder.Ascending });

            string answer = "";
            foreach (var m in msgs)
            {
                if (m.Role == MessageRole.Assistant)
                {
                    foreach (var c in m.Content)
                    {
                        if (!string.IsNullOrEmpty(c.Text))
                            answer += c.Text + "\n";
                    }
                }
            }

            return new MessageToMethodResult()
            {
                isSuccess = true,
                isPrintMessage = true,
                message = answer.Trim()
            };
        }


        /// <summary>
        /// 벡터스토어 Poll
        /// </summary>
        private static async Task<VectorStore> PollVectorStoreAsync(
            VectorStoreClient client,
            string vectorStoreId)
        {
            while (true)
            {
                ClientResult<VectorStore> result = await client.GetVectorStoreAsync(vectorStoreId);
                var store = result.Value;
                var response = result.GetRawResponse();

                if (store.Status == VectorStoreStatus.InProgress)
                {
                    TimeSpan pollDelay = TimeSpan.FromSeconds(1);
                    if (response.Headers.TryGetValue("Retry-After", out var retryAfter))
                    {
                        if (int.TryParse(retryAfter, out var delaySeconds))
                            pollDelay = TimeSpan.FromSeconds(delaySeconds);
                    }
                    await Task.Delay(pollDelay);
                }
                else
                {
                    return store;
                }
            }
        }
    }
}

#pragma warning restore OPENAI001


//using OpenAI.Chat;
//using OpenAI.Responses;
//using System;
//using System.IO;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Test_2.Base;
//using Test_2.Code.Object;
//using OpenAI;
//using OpenAI.Files;
//using OpenAI.VectorStores;
//using System.IO;
//using System.ClientModel;
//using System.Reflection.Metadata;
//using OpenAI.Assistants;

//#pragma warning disable OPENAI001

//namespace Test_2.Apps.LlmChat
//{
//    internal class LlmOpenAi : IMessageToMethod
//    {
//        public LlmOpenAi()
//        {

//        }


//        public async Task FileUpload()
//        {
//            OpenAIClient openAIClient = Singleton.instance.openAIClient;
//            OpenAIFileClient fileClient = openAIClient.GetOpenAIFileClient();
//            AssistantClient assistantClient = openAIClient.GetAssistantClient();

//            List<OpenAIFile> files = new List<OpenAIFile>();
//            VectorStoreCreationOptions storeOptions = new VectorStoreCreationOptions();

//            foreach (string path in Singleton.instance.mdFilePaths)
//            {
//                Stream document = new FileStream(
//                    $"./{path}",      // 상대 경로 파일
//                    FileMode.Open,
//                    FileAccess.Read,
//                    FileShare.Read
//                    );

//                OpenAIFile file = await fileClient.UploadFileAsync(
//                    document,
//                    $"{path}",
//                    FileUploadPurpose.Assistants);
//                files.Add(file);
//            }

//            VectorStore store = await Singleton.instance.vectorStoreClient.CreateVectorStoreAsync(storeOptions);




//            //var api = new OpenAIClient();
//            //using var stream = File.OpenRead("data.json");
//            //var file = await api.Files.UploadAsync(
//            //    file: stream,
//            //    fileName: "data.json",
//            //    purpose: Purpose.Assistants
//            //);

//            //Singleton.instance.
//        }

//        /// <summary>
//        ///     파일을 업로드합니다.
//        /// </summary>
//        /// <param name="client"></param>
//        /// <param name="vectorStoreId"></param>
//        /// <returns></returns>
//        private static async Task<VectorStore> PollVectorStoreAsync(
//            VectorStoreClient client,
//            string vectorStoreId)
//        {
//            while (true)
//            {
//                ClientResult<VectorStore> result = await client.GetVectorStoreAsync(vectorStoreId);
//                var store = result.Value;
//                var response = result.GetRawResponse();

//                if (store.Status == VectorStoreStatus.InProgress)
//                {
//                    // Default 1s; override if server hints are present
//                    TimeSpan pollDelay = TimeSpan.FromSeconds(1);
//                    if (response.Headers.TryGetValue("Retry-After", out var retryAfter))
//                    {
//                        if (int.TryParse(retryAfter, out var delaySeconds))
//                        {
//                            pollDelay = TimeSpan.FromSeconds(delaySeconds);
//                        }
//                        else if (DateTimeOffset.TryParse(retryAfter, out var retryAfterDate))
//                        {
//                            pollDelay = retryAfterDate - DateTimeOffset.Now;
//                        }
//                    }
//                    else if (response.Headers.TryGetValue("openai-poll-after-ms", out var msStr) &&
//                            int.TryParse(msStr, out var ms))
//                    {
//                        pollDelay = TimeSpan.FromMilliseconds(ms);
//                    }

//                    await Task.Delay(pollDelay);
//                }
//                else
//                {
//                    return store;
//                }
//            }
//        }

//        public async Task<MessageToMethodResult> AssignWorkAsync(string message)
//        {
//            string[] commands = message.Split(' ');

//            MessageToMethodResult result = new MessageToMethodResult()
//            {
//                isSuccess = true,
//                message = message
//            };

//            ChatCompletion completion =
//                await Singleton.instance.chatClient.CompleteChatAsync(
//                    message.Substring(commands[0].Length)
//                    );

//            if (completion.Content[0].Text.Length > 0)
//            {
//                result = new MessageToMethodResult()
//                {
//                    isSuccess = true,
//                    isPrintMessage = true,
//                    message = completion.Content[0].Text
//                };
//            }

//            return result;
//        }

//        //public async Task<MessageToMethodResult> Test()
//        //{
//        //    OpenAIResponseClient client = new(
//        //        model: "gpt-4o-mini",
//        //        apiKey: TokenReceiver.GetApiKey(EKeyType.OpenAiApi));

//        //    //ResponseTool fileSearchTool
//        //    //    = ResponseTool.CreateFileSearchTool(
//        //    //        vectorStoreIds: [ExistingVectorStoreForTest.Id]);
//        //    //OpenAIResponse response = await client.CreateResponseAsync(
//        //    //    userInputText: "According to available files, what's the secret number?",
//        //    //    new ResponseCreationOptions()
//        //    //    {
//        //    //        Tools = { fileSearchTool }
//        //    //    });

//        //    //foreach (ResponseItem outputItem in response.OutputItems)
//        //    //{
//        //    //    if (outputItem is FileSearchCallResponseItem fileSearchCall)
//        //    //    {
//        //    //        Console.WriteLine($"[file_search] ({fileSearchCall.Status}): {fileSearchCall.Id}");
//        //    //        foreach (string query in fileSearchCall.Queries)
//        //    //        {
//        //    //            Console.WriteLine($"  - {query}");
//        //    //        }
//        //    //    }
//        //    //    else if (outputItem is MessageResponseItem message)
//        //    //    {
//        //    //        Console.WriteLine($"[{message.Role}] {message.Content.FirstOrDefault()?.Text}");
//        //    //    }
//        //    //}
//        //}
//    }
//}

//#pragma warning restore OPENAI001