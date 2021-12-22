using iCrabee.ViewModels;
using iCrabee.ViewModels.Contents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iCrabee.WebPortal.Services
{
    public class KnowledgeBaseApiClient : BaseApiClient, IKnowledgeBaseApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public KnowledgeBaseApiClient(IHttpClientFactory httpClientFactory,
          IConfiguration configuration,
          IHttpContextAccessor httpContextAccessor) : base(httpClientFactory, configuration, httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Pagination<CommentVM>> GetCommentsTree(int knowledgeBaseId, int pageIndex, int pageSize)
        {
            return await GetAsync<Pagination<CommentVM>>($"/api/knowledgeBases/{knowledgeBaseId}/comments/tree?pageIndex={pageIndex}&pageSize={pageSize}");
        }

        public async Task<Pagination<CommentVM>> GetRepliedComments(int knowledgeBaseId, int rootCommentId, int pageIndex, int pageSize)
        {
            return await GetAsync<Pagination<CommentVM>>($"/api/knowledgeBases/{knowledgeBaseId}/comments/{rootCommentId}/replied?pageIndex={pageIndex}&pageSize={pageSize}");
        }

        public async Task<KnowledgeBaseVM> GetKnowledgeBaseDetail(int id)
        {
            return await GetAsync<KnowledgeBaseVM>($"/api/knowledgeBases/{id}");
        }

        public async Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByCategoryId(int categoryId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?categoryId={categoryId}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVM>>(apiUrl);
        }

        public async Task<Pagination<KnowledgeBaseQuickVM>> GetKnowledgeBasesByTagId(string tagId, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/tags/{tagId}?pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVM>>(apiUrl);
        }

        public async Task<List<LabelVM>> GetLabelsByKnowledgeBaseId(int id)
        {
            return await GetListAsync<LabelVM>($"/api/knowledgeBases/{id}/labels");
        }

        public async Task<List<KnowledgeBaseQuickVM>> GetLatestKnowledgeBases(int take)
        {
            return await GetListAsync<KnowledgeBaseQuickVM>($"/api/knowledgeBases/latest/{take}");
        }

        public async Task<List<KnowledgeBaseQuickVM>> GetPopularKnowledgeBases(int take)
        {
            return await GetListAsync<KnowledgeBaseQuickVM>($"/api/knowledgeBases/popular/{take}");
        }

        public async Task<List<CommentVM>> GetRecentComments(int take)
        {
            return await GetListAsync<CommentVM>($"/api/knowledgeBases/comments/recent/{take}");
        }

        public async Task<CommentVM> PostComment(CommentCreateRequest request)
        {
            return await PostAsync<CommentCreateRequest, CommentVM>($"/api/knowledgeBases/{request.KnowledgeBaseId}/comments", request);
        }

        public async Task<bool> PostKnowlegdeBase(KnowledgeBaseCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            using var requestContent = new MultipartFormDataContent();

            if (request.Attachments?.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    byte[] data;
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "attachments", item.FileName);
                }
            }
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
            requestContent.Add(new StringContent(request.Title.ToString()), "title");
            requestContent.Add(new StringContent(request.Problem.ToString()), "problem");
            requestContent.Add(new StringContent(request.Note.ToString()), "note");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Environment.ToString()), "environment");
            requestContent.Add(new StringContent(request.StepToReproduce.ToString()), "stepToReproduce");
            requestContent.Add(new StringContent(request.ErrorMessage.ToString()), "errorMessage");
            requestContent.Add(new StringContent(request.Workaround.ToString()), "workaround");
            if (request.Labels?.Length > 0)
            {
                foreach (var label in request.Labels)
                {
                    requestContent.Add(new StringContent(label), "labels");
                }
            }

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsync($"/api/knowledgeBases/", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> PutKnowlegdeBase(int id, KnowledgeBaseCreateRequest request)
        {
            var client = _httpClientFactory.CreateClient("BackendApi");

            client.BaseAddress = new Uri(_configuration["BackendApiUrl"]);
            using var requestContent = new MultipartFormDataContent();

            if (request.Attachments?.Count > 0)
            {
                foreach (var item in request.Attachments)
                {
                    byte[] data;
                    using (var br = new BinaryReader(item.OpenReadStream()))
                    {
                        data = br.ReadBytes((int)item.OpenReadStream().Length);
                    }
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    requestContent.Add(bytes, "attachments", item.FileName);
                }
            }
            requestContent.Add(new StringContent(request.CategoryId.ToString()), "categoryId");
            requestContent.Add(new StringContent(request.Title.ToString()), "title");
            requestContent.Add(new StringContent(request.Problem.ToString()), "problem");
            requestContent.Add(new StringContent(request.Note.ToString()), "note");
            requestContent.Add(new StringContent(request.Description.ToString()), "description");
            requestContent.Add(new StringContent(request.Environment.ToString()), "environment");
            requestContent.Add(new StringContent(request.StepToReproduce.ToString()), "stepToReproduce");
            requestContent.Add(new StringContent(request.ErrorMessage.ToString()), "errorMessage");
            requestContent.Add(new StringContent(request.Workaround.ToString()), "workaround");
            if (request.Labels?.Length > 0)
            {
                foreach (var label in request.Labels)
                {
                    requestContent.Add(new StringContent(label), "labels");
                }
            }

            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"/api/knowledgeBases/{id}", requestContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<Pagination<KnowledgeBaseQuickVM>> SearchKnowledgeBase(string keyword, int pageIndex, int pageSize)
        {
            var apiUrl = $"/api/knowledgeBases/filter?filter={keyword}&pageIndex={pageIndex}&pageSize={pageSize}";
            return await GetAsync<Pagination<KnowledgeBaseQuickVM>>(apiUrl);
        }

        public async Task<bool> UpdateViewCount(int id)
        {
            return await PutAsync<object, bool>($"/api/knowledgeBases/{id}/view-count", null, false);
        }

        public async Task<int> PostVote(VoteCreateRequest request)
        {
            return await PostAsync<VoteCreateRequest, int>($"/api/knowledgeBases/{request.KnowledgeBaseId}/votes", null);
        }

        public async Task<ReportVM> PostReport(ReportCreateRequest request)
        {
            return await PostAsync<ReportCreateRequest, ReportVM>($"/api/knowledgeBases/{request.KnowledgeBaseId}/reports", request);
        }
    }
}