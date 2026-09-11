using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_2.Apps.Works.Automation.SingleTask.JobSearch
{
    internal class WorkAutomationRoutineJobSearch : IWorkAutomationRoutine
    {
        string url = "https://www.gamejob.co.kr/Recruit/joblist?menucode=duty&duty=1";


        public async Task<WorkAutomationRoutineResult> StartAsync(WorkAutomationRoutineArguments arguments)
        {
            DateTime a = new DateTime();


            Hack.Say(this, "호출되었습니다.");
            //await Task.Delay(1000);
            WorkAutomationRoutineResult result = new WorkAutomationRoutineResult();
            return result;
        }

        // 웹 페이지의 HTML을 가져오는 메서드
        async Task<string> GetHtmlAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0"); // 웹 서버 차단 방지
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        Hack.Say(this, $"Error: {response.StatusCode}");
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Hack.Say(this, $"Request failed: {ex.Message}");
                    return string.Empty;
                }
            }
        }
    }
}
