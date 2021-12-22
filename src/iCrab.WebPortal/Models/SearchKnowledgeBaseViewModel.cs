using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;

namespace iCrabee.WebPortal.Models
{
    public class SearchKnowledgeBaseViewModel
    {
        public Pagination<KnowledgeBaseQuickVM> Data { set; get; }

        public string Keyword { set; get; }
    }
}
