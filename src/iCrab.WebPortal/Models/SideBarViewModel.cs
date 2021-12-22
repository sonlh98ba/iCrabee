using iCrabee.ViewModels.Contents;
using System.Collections.Generic;

namespace iCrabee.WebPortal.Models
{
    public class SideBarViewModel
    {
        public List<KnowledgeBaseQuickVM> PopularKnowledgeBases { get; set; }

        public List<CategoryVM> Categories { get; set; }

        public List<CommentVM> RecentComments { get; set; }
    }
}
