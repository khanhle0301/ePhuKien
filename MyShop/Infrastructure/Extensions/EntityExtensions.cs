using Model.EF;
using MyShop.Models;

namespace MyShop.Infrastructure.Extensions
{
    public static class EntityExtensions
    {
        public static void UpdatePostCategory(this PostCategory postCategory, PostCategoryViewModel postCategoryVm)
        {
            postCategory.ID = postCategoryVm.ID;
            postCategory.Name = postCategoryVm.Name;         
            postCategory.Alias = postCategoryVm.Alias;           
            postCategory.CreatedDate = postCategoryVm.CreatedDate;
            postCategory.CreatedBy = postCategoryVm.CreatedBy;
            postCategory.UpdatedDate = postCategoryVm.UpdatedDate;
            postCategory.UpdatedBy = postCategoryVm.UpdatedBy;
            postCategory.MetaKeyword = postCategoryVm.MetaKeyword;
            postCategory.MetaDescription = postCategoryVm.MetaDescription;
            postCategory.Status = postCategoryVm.Status;
        }

        public static void UpdateProductCategory(this ProductCategory productCategory, ProductCategoryViewModel productCategoryVm)
        {
            productCategory.ID = productCategoryVm.ID;
            productCategory.Name = productCategoryVm.Name;
            productCategory.Alias = productCategoryVm.Alias;
            productCategory.ParentID = productCategoryVm.ParentID;
            productCategory.DisplayOrder = productCategoryVm.DisplayOrder;
            productCategory.Image = productCategoryVm.Image;
            productCategory.CreatedDate = productCategoryVm.CreatedDate;
            productCategory.CreatedBy = productCategoryVm.CreatedBy;
            productCategory.UpdatedDate = productCategoryVm.UpdatedDate;
            productCategory.UpdatedBy = productCategoryVm.UpdatedBy;
            productCategory.MetaKeyword = productCategoryVm.MetaKeyword;
            productCategory.MetaDescription = productCategoryVm.MetaDescription;
            productCategory.Status = productCategoryVm.Status;
        }

        public static void UpdatePost(this Post post, PostViewModel postVm)
        {
            post.ID = postVm.ID;
            post.Name = postVm.Name;
            post.Description = postVm.Description;
            post.Alias = postVm.Alias;
            post.CategoryID = postVm.CategoryID;
            post.Content = postVm.Content;
            post.Image = postVm.Image;           
            post.ViewCount = postVm.ViewCount;
            post.CreatedDate = postVm.CreatedDate;
            post.CreatedBy = postVm.CreatedBy;
            post.UpdatedDate = postVm.UpdatedDate;
            post.UpdatedBy = postVm.UpdatedBy;
            post.MetaKeyword = postVm.MetaKeyword;
            post.MetaDescription = postVm.MetaDescription;
            post.Status = postVm.Status;
            post.Tags = postVm.Tags;
        }

        public static void UpdateProduct(this Product product, ProductViewModel productVm)
        {
            product.ID = productVm.ID;
            product.Name = productVm.Name;
            product.Description = productVm.Description;
            product.Alias = productVm.Alias;
            product.CategoryID = productVm.CategoryID;          
            product.Content = productVm.Content;
            product.Quantity = productVm.Quantity;
            product.QuantitySold = productVm.QuantitySold;
            product.Image = productVm.Image;
            product.Image2 = productVm.Image2;
            product.MoreImages = productVm.MoreImages;
            product.Price = productVm.Price;
            product.PromotionPrice = productVm.PromotionPrice;
            product.Warranty = productVm.Warranty;
            product.HomeFlag = productVm.HomeFlag;
            product.HotFlag = productVm.HotFlag;
            product.ViewCount = productVm.ViewCount;
            product.CreatedDate = productVm.CreatedDate;
            product.CreatedBy = productVm.CreatedBy;
            product.UpdatedDate = productVm.UpdatedDate;
            product.UpdatedBy = productVm.UpdatedBy;
            product.MetaKeyword = productVm.MetaKeyword;
            product.MetaDescription = productVm.MetaDescription;
            product.Status = productVm.Status;
            product.Tags = productVm.Tags;                   
        }

        public static void UpdateSlide(this Slide slide, SlideViewModel slideVm)
        {
            slide.ID = slideVm.ID;
            slide.Name = slideVm.Name;
            slide.Description = slideVm.Description;           
            slide.DisplayOrder = slideVm.DisplayOrder;
            slide.Image = slideVm.Image;
            slide.Url = slideVm.Url;
            slide.Status = slideVm.Status;
        }       

        public static void UpdatePage(this Page page, PageViewModel pageVm)
        {
            page.ID = pageVm.ID;
            page.Name = pageVm.Name;        
            page.Alias = pageVm.Alias;
            page.DisplayOrder = pageVm.DisplayOrder;
            page.Content = pageVm.Content;           
            page.CreatedDate = pageVm.CreatedDate;
            page.CreatedBy = pageVm.CreatedBy;
            page.UpdatedDate = pageVm.UpdatedDate;
            page.UpdatedBy = pageVm.UpdatedBy;
            page.MetaKeyword = pageVm.MetaKeyword;
            page.MetaDescription = pageVm.MetaDescription;
            page.Status = pageVm.Status;            
        }

        public static void UpdateContactDetail(this ContactDetail contactDetail, ContactDetailViewModel contactDetailVm)
        {
            contactDetail.ID = contactDetailVm.ID;
            contactDetail.Name = contactDetailVm.Name;
            contactDetail.Phone = contactDetailVm.Phone;
            contactDetail.Email = contactDetailVm.Email;
            contactDetail.Website = contactDetailVm.Website;
            contactDetail.Address = contactDetailVm.Address;
            contactDetail.Other = contactDetailVm.Other;
            contactDetail.Lat = contactDetailVm.Lat;
            contactDetail.Lng = contactDetailVm.Lng;           
            contactDetail.Status = contactDetailVm.Status;
        }
        public static void UpdateFeedback(this Feedback feedback, FeedbackViewModel feedbackVm)
        {
            feedback.ID = feedbackVm.ID;
            feedback.Name = feedbackVm.Name;
            feedback.Email = feedbackVm.Email;
            feedback.Phone = feedbackVm.Phone;
            feedback.Message = feedbackVm.Message;
            feedback.CreatedDate = feedbackVm.CreatedDate;           
            feedback.Status = feedbackVm.Status;
        }

        public static void UpdateFooter(this Footer footer, FooterViewModel footerVm)
        {
            footer.ID = footerVm.ID;
            footer.Name = footerVm.Name;
            footer.Content = footerVm.Content;            
        }


        public static void UpdateUserGroup(this UserGroup userGr, UserGroupViewModel userGrVm)
        {
            userGr.ID = userGrVm.ID;
            userGr.Name = userGrVm.Name;            
        }

        public static void UpdateUser(this User user, UserViewModel userVm)
        {
            user.ID = userVm.ID;
            user.Name = userVm.Name;
            user.GroupID = userVm.GroupID;
            user.Address = userVm.Address;
            user.Email = userVm.Email;
            user.Phone = userVm.Phone;          
        }

        public static void UpdateOrder(this Order order, OrderViewModel orderVm)
        {
            order.ID = orderVm.ID;
            order.CustomerName = orderVm.CustomerName;
            order.CustomerEmail = orderVm.CustomerEmail;
            order.CustomerAddress = orderVm.CustomerAddress;
            order.CustomerMobile = orderVm.CustomerMobile;
            order.CustomerMessage = orderVm.CustomerMessage;
            order.PaymentMethod = orderVm.PaymentMethod;
            order.PaymentStatus = orderVm.PaymentStatus;           
            order.CreatedDate = orderVm.CreatedDate;
            order.Status = orderVm.Status;
        }

        public static void UpdateOrderDetail(this OrderDetail orderDetail, OrderDetailViewModel orderDetailVm)
        {
            orderDetail.OrderID = orderDetailVm.OrderID;
            orderDetail.ProductID = orderDetailVm.ProductID;
            orderDetail.Quantitty = orderDetailVm.Quantitty;
            orderDetail.Note = orderDetailVm.Note;
            orderDetail.Price = orderDetailVm.Price;
        }     

        public static void UpdateColor(this Color color, ColorViewModel colorVm)
        {
            color.ID = colorVm.ID;
            color.Name = colorVm.Name;
            color.Background = colorVm.Background;            
        }

    }
}