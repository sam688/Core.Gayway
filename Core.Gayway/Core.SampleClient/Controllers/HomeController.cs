using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Core.SampleClient.Models;
using Consul;

namespace Core.SampleClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        private static int iSeed = 0;
        public IActionResult Index()
        {
            
            try
            {
                //string url = "https://service.sample.https/WeatherForecast";
                string url = "https://service.sample.https/user/getargs";
                //string url = "https://service.sample.https/home/index";

                string conent = "";
                Uri uri = new Uri(url);
                string host = uri.Host;


                using (ConsulClient client = new ConsulClient(c =>
                {
                    c.Address = new Uri("http://localhost:8500/");
                    c.Datacenter = "dc1";
                }))
                {
                    //var distionary = client.Agent.Services().Result.Response;
                    //foreach (var kv in distionary)
                    //{
                    //    AgentService agentService = kv.Value;
                    //    string message = $"{agentService.Address}; {agentService.Port}; {agentService.ID}; {agentService.Service}";
                    //    _logger.LogInformation(message);
                    //}

                    string targetUrl = "";
                    var distionary = client.Agent.Services().Result.Response;
                    if (distionary.Count > 0)
                    {
                        var list = distionary.Where(k => k.Value.Service.Equals(host, StringComparison.OrdinalIgnoreCase));
                        KeyValuePair<string, AgentService> keyValuePair = new KeyValuePair<string, AgentService>();

                        //keyValuePair = list.First(); //只取第一个
                        //keyValuePair = list.ToArray()[new Random(iSeed++).Next(0, list.Count())];//均衡策略
                        //keyValuePair = list.ToArray()[iSeed++%list.Count()];//轮徇策略

                        #region 均衡策略 start
                        List<KeyValuePair<string, AgentService>> serviceList = new List<KeyValuePair<string, AgentService>>();
                        foreach (var pair in list)
                        {
                            //权重策略
                            AgentService agentService = pair.Value;
                            string[] tags = agentService.Tags;
                            int weight = 0;
                            try
                            {
                                if (tags != null && tags.Length > 0)
                                    weight = int.Parse(tags[tags.Length - 1]);
                            }
                            catch(Exception ex)
                            {
                                _logger.LogError(ex.Message);
                            }

                            //加权
                            for (int i = 0; i < weight; i++)
                            {
                                serviceList.Add(pair);
                            }

                            //string message = $"{agentService.Address}; {agentService.Port}; {agentService.ID}; {agentService.Service}";
                            //_logger.LogInformation(message);
                        }

                        iSeed = iSeed > 100000 ? 0 : iSeed;
                        int index = new Random(iSeed++).Next(0, serviceList.Count());

                        keyValuePair = serviceList.ToArray()[index];//均衡策略
                        #endregion

                        targetUrl = $"{uri.Scheme}://{keyValuePair.Value.Address}:{keyValuePair.Value.Port}{uri.PathAndQuery}";
                        base.ViewBag.ServiceUrl = targetUrl;

                        _logger.LogInformation(targetUrl);
                        conent = HttpClientUtil.Get(targetUrl);
                        //_logger.LogInformation(targetUrl + "=>" + conent);
                        base.ViewBag.Content = conent;

                    }
                    
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View();
        }

        public IActionResult Privacy()
        {
            try
            {
                string targetUrl = "https://localhost:44347/user/getargs";
                base.ViewBag.ServiceUrl = targetUrl;

                _logger.LogInformation(targetUrl);
                string conent = HttpClientUtil.Get(targetUrl); 
                //_logger.LogInformation(targetUrl + "=>" + conent);
                base.ViewBag.Content = conent;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
