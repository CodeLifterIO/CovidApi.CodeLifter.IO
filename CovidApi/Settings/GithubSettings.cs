using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CovidApi.Settings
{
    public class GithubSettings
    {
        public string GithubFolderPath { get; set; }
        public string ProductHeaderValue { get; set; }
        public string Token { get; set; }
        public string RepoOwner { get; set; }
        public string RepoName { get; set; }
    }
}
