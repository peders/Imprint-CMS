using System.Linq;

namespace ImprintCMS.Models
{
    public class AdminConfig
    {
        public string TinyEditorApiKey { get; private set; }

        public AdminConfig(Repository repository)
        {
            var config = repository.Configurations.SingleOrDefault(_ => _.IsActive);
            if (config != null)
            {
                TinyEditorApiKey = config.HtmlEditorApiKey ?? string.Empty;
            }
        }

    }

}