using AutoMapper;
using Common;
using Model.Dao;
using Model.EF;
using MyShop.Infrastructure.Core;
using MyShop.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MyShop.Controllers
{
    public class PostController : Controller
    {
        private PostDao _postDao = new PostDao();
        private PostCategoryDao _postCategory = new PostCategoryDao();
        private TagDao _tagDao = new TagDao();

        // GET: Post
        public ActionResult Index(int page = 1)
        {
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postDao.GetAllPaging(page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        public ActionResult Category(string alias, int page = 1)
        {
            var category = _postCategory.GetByAlias(alias);
            ViewBag.Category = Mapper.Map<PostCategory, PostCategoryViewModel>(category);
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postDao.GetAllByCategoryPaging(category.ID, page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        public ActionResult ListByTag(string tagId, int page = 1)
        {
            ViewBag.Tags = _tagDao.GetById(tagId);
            int pageSize = int.Parse(ConfigHelper.GetByKey("PageSizePost"));
            int totalRow = 0;
            var postModel = _postDao.GetAllByTagPaging(tagId, page, pageSize, out totalRow);
            var postViewModel = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(postModel);
            int totalPage = (int)Math.Ceiling((double)totalRow / pageSize);
            var paginationSet = new PaginationSet<PostViewModel>()
            {
                Items = postViewModel,
                MaxPage = int.Parse(ConfigHelper.GetByKey("MaxPage")),
                Page = page,
                TotalCount = totalRow,
                TotalPages = totalPage
            };
            return View(paginationSet);
        }

        [ChildActionOnly]
        public ActionResult TopNewPost()
        {
            var model = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(_postDao.GetNew(3));
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostTag()
        {
            var model = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_tagDao.GetPostTags());
            return PartialView(model);
        }

        [ChildActionOnly]
        public ActionResult PostCategory()
        {
            var model = Mapper.Map<IEnumerable<PostCategory>, IEnumerable<PostCategoryViewModel>>(_postCategory.GetPostCategroy());
            return PartialView(model);
        }

        public ActionResult Detail(int id)
        {
            var post = _postDao.GetAllById(id);
            var viewModel = Mapper.Map<Post, PostViewModel>(post);
            ViewBag.Tags = Mapper.Map<IEnumerable<Tag>, IEnumerable<TagViewModel>>(_postDao.GetListTagByPostId(id));          
            var relatedPost = _postDao.GetReatedPosts(id, 2);
            ViewBag.RelatedPosts = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(relatedPost);
            var relatedTakePost = _postDao.GetReatedTakePosts(id, 2);
            ViewBag.RelatedTakePosts = Mapper.Map<IEnumerable<Post>, IEnumerable<PostViewModel>>(relatedTakePost);
            _postDao.IncreaseView(id);
            return View(viewModel);
        }
    }
}