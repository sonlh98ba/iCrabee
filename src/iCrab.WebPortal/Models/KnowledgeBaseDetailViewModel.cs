using iCrabee.ViewModels.Contents;
using iCrabee.ViewModels.Systems;
using System.Collections.Generic;

namespace iCrabee.WebPortal.Models
{
    public class KnowledgeBaseDetailViewModel
    {
        public CategoryVM Category { set; get; }

        public KnowledgeBaseVM Detail { get; set; }

        public List<LabelVM> Labels { get; set; }

        public UserVM CurrentUser { get; set; }
    }
}
