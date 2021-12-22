using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public interface IKnowledgeBaseApiClient
    {
        Task<List<KnowledgeBaseQuickVM>> GetPopularKnowledgeBases(int take);

        Task<List<KnowledgeBaseQuickVM>> GetLatestKnowledgeBases(int take);

        Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize);

        Task<Pagination<KnowledgeBaseQuickVM>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize);

        Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize);

        Task<KnowledgeBaseVM> GetKnowledgeBaseDetail(int id);

        Task<List<LabelVM>> GetLabelsByKnowledgeBaseId(int id);

        Task<List<CommentVM>> GetRecentComments(int take);

        Task<Pagination<CommentVM>> GetCommentsTree(int knowledgeBaseId, int pageIndex, int pageSize);

        Task<Pagination<CommentVM>> GetRepliedComments(int knowledgeBaseId, int rootCommentId, int pageIndex, int pageSize);

        Task<CommentVM> PostComment(CommentCreateRequest request);

        Task<bool> PostKnowlegdeBase(KnowledgeBaseCreateRequest request);

        Task<bool> PutKnowlegdeBase(int id, KnowledgeBaseCreateRequest request);

        Task<bool> UpdateViewCount(int id);

        Task<int> PostVote(VoteCreateRequest request);

        Task<ReportVM> PostReport(ReportCreateRequest request);
    }
}
