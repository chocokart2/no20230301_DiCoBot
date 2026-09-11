using Google.Apis.Auth.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation.SingleTask.SolvedAcCoinNotification
{
    internal class WorkAutomationRoutineSolvedAcCoinNotification : IWorkAutomationRoutine
    {
        public class ExchangeRate
        {
            public int rate { get; set; }
        }


        public async Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments)
        {
            using (HttpClient client = new HttpClient())
            {
                // 요청 URL
                string url = "https://solved.ac/api/v3/coins/exchange_rate";

                // 요청 메시지 생성
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/json");

                try
                {
                    WorkAutomationRoutineResult m_result = new WorkAutomationRoutineResult();

                    // 요청 보내기
                    HttpResponseMessage response = await client.SendAsync(request);

                    // 응답 성공 여부 확인
                    if (response.IsSuccessStatusCode)
                    {
                        // 응답 본문 읽기
                        string responseBody = await response.Content.ReadAsStringAsync();
                        ExchangeRate resultObject = JsonSerializer.Deserialize<ExchangeRate>(responseBody);
                        Console.WriteLine($"솔브닷에서 받은 내용\n{responseBody}\n역직렬화 후 멤버 : {resultObject.rate}");


                        if (resultObject == null )
                        {
                            m_result.SetMessage("역직렬화 실패!");
                            m_result.SetDiscordMessage("역직렬화 실패!");
                            m_result.Seal();
                            return m_result;
                        }
                        else
                        {
                            m_result.SetMessage(resultObject.rate.ToString());
                            m_result.SetDiscordMessage(resultObject.rate.ToString());
                            m_result.Seal();
                            return m_result;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }

            return new WorkAutomationRoutineResult();
        }
    }
}
