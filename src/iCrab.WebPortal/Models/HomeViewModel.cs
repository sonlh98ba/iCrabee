using iCrabee.ViewModels.Contents;
using System.Collections.Generic;

namespace iCrabee.WebPortal.Models
{
    public class HomeViewModel
    {
        public List<KnowledgeBaseQuickVM> LatestKnowledgeBases { get; set; }
        public List<KnowledgeBaseQuickVM> PopularKnowledgeBases { get; set; }

        public List<LabelVM> PopularLabels { get; set; }
    }
}
