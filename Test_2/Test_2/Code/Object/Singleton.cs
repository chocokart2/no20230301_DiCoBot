using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using OpenAI;
using OpenAI.Assistants;
using OpenAI.Chat;
using OpenAI.Files;
using OpenAI.VectorStores;
using Test_2.Apps.LlmChat;
using Test_2.Apps.NotionFileReceiver;

namespace Test_2.Code.Object
{
    internal class Singleton
    {
        public static Singleton instance;

        // 배열
        public List<string> mdFilePaths;

        // 내가 만든 클래스
        public LlmOpenAi llmOpenAi;
        public NotionFileGetter notionFileGetter;

        // 외부 API 클래스
        // notion
        public HttpClient notionHttpClient;

        // openAI
        public OpenAIClient openAIClient;
        public ChatClient chatClient;
        public VectorStoreClient vectorStoreClient;
        public OpenAIFileClient fileClient;
        public AssistantClient assistantClient;
        public Assistant assistant;

        


        public void Init()
        {
            mdFilePaths = new List<string>()
            {
                ""
            };

            //notionHttpClient = 



            chatClient = new ChatClient(
                "gpt-4o",
                TokenReceiver.GetApiKey(EKeyType.OpenAiApi)
                );
            openAIClient = new OpenAIClient(
                TokenReceiver.GetApiKey(EKeyType.OpenAiApi)
                );
            vectorStoreClient = openAIClient.GetVectorStoreClient();
            llmOpenAi = new LlmOpenAi();

            fileClient = openAIClient.GetOpenAIFileClient();
            assistantClient = openAIClient.GetAssistantClient();

            notionFileGetter = new NotionFileGetter();


            
            //OpenAIFile salesFile = fileClient.UploadFile(
            //    document,
            //    "monthly_sales.json",
            //    FileUploadPurpose.Assistants);

            //AssistantCreationOptions assistantOptions = new()
            //{
            //    Name = "Example: Contoso sales RAG",
            //    Instructions =
            //        "당신은 게임 기획서가 적힌 노션 JSON 파일을 읽고 이해하는 어시스턴트입니다."
            //        + " 사용자가 질문하면, 반드시 해당 노션 JSON 파일의 내용에 기반해서 한국어 두루높임 비격식체로 답변해야 합니다."
            //        + " 만약 질문과 관련된 내용이 JSON 파일에 없으면, \"해당 내용이 JSON 파일에 없습니다\"라고 답변해야 합니다."
            //        + " 사용자가 개선사항을 요구한다면, 필요하다면 참고용 일반적인 안내를 덧붙일 수 있습니다.",
            //    Tools =
            //    {
            //        new FileSearchToolDefinition(),
            //        new CodeInterpreterToolDefinition(),
            //    },
            //    ToolResources = new()
            //    {
            //        FileSearch = new()
            //        {
            //            NewVectorStores =
            //            {
            //                new VectorStoreCreationHelper(new string[] {salesFile.Id}),
            //            }
            //        }
            //    },
            //};

            //Assistant assistant = assistantClient.CreateAssistant("gpt-4o", assistantOptions);


            //ChatCompletion completion = chatClient.CompleteChat("고양이에 대한 설명을 한 줄로 해 줘");

            //Console.WriteLine($"[ASSISTANT]: {completion.Content[0].Text}");

            //var aaa = new OpenAIClient(TokenReceiver.GetApiKeyOrNull(EKeyType.OpenAiApi));
            //var bbb = aaa.Chat;
        }







    }
}
