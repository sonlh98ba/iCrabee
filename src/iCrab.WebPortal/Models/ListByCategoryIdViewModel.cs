using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;

namespace iCrabee.WebPortal.Models
{
    public class ListByCategoryIdViewModel
    {
        public Pagination<KnowledgeBaseQuickVM> Data { set; get; }

        public CategoryVM Category { set; get; }
    }
}
