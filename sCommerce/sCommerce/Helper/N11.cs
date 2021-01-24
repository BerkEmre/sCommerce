using sCommerce.com.n11Category.api;
using sCommerce.com.n11Product.api;
using sCommerce.com.n11Shipment.api;
using System.Configuration;
using sCommerce.Models;
using System.Web;

namespace sCommerce.Helper
{
    public class N11
    {
        string apiKey = ConfigurationManager.AppSettings["n11_apikey"];
        string apiSecret = ConfigurationManager.AppSettings["n11_secretkey"];

        public SubCategory[] GetTopCategories()
        {
            com.n11Category.api.Authentication authentication = new com.n11Category.api.Authentication();
            authentication.appKey = apiKey;
            authentication.appSecret = apiSecret;
            
            GetTopLevelCategoriesRequest getTopLevelCategories = new GetTopLevelCategoriesRequest();
            getTopLevelCategories.auth = authentication;

            CategoryServicePortService port = new CategoryServicePortService();
            GetTopLevelCategoriesResponse getTopLevelCategoriesResponse = port.GetTopLevelCategories(getTopLevelCategories);
            SubCategory[] subCategories = getTopLevelCategoriesResponse.categoryList;

            return subCategories;
        }

        public SubCategory[] GetSubCategories(long categoryId)
        {
            com.n11Category.api.Authentication authentication = new com.n11Category.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            GetSubCategoriesRequest getSubCategoriesRequest = new GetSubCategoriesRequest();
            getSubCategoriesRequest.auth = authentication;
            getSubCategoriesRequest.categoryId = categoryId;

            CategoryServicePortService port = new CategoryServicePortService();
            GetSubCategoriesResponse getSubCategoriesResponse = port.GetSubCategories(getSubCategoriesRequest);
            SubCategory[] subCategories = getSubCategoriesResponse.category[0].subCategoryList;

            return subCategories;
        }

        public ParentCategoryData GetParentCategory(long categoryId)
        {
            com.n11Category.api.Authentication authentication = new com.n11Category.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            GetParentCategoryRequest getParentCategoryRequest = new GetParentCategoryRequest();
            getParentCategoryRequest.auth = authentication;
            getParentCategoryRequest.categoryId = categoryId;

            CategoryServicePortService port = new CategoryServicePortService();
            GetParentCategoryResponse getParentCategoryResponse = port.GetParentCategory(getParentCategoryRequest);
            ParentCategoryData category = getParentCategoryResponse.category;

            return category;
        }
    
        public ShipmentApiModel[] GetShipmentTemplateList()
        {
            com.n11Shipment.api.Authentication authentication = new com.n11Shipment.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            GetShipmentTemplateListRequest getShipmentTemplateListRequest = new GetShipmentTemplateListRequest();
            getShipmentTemplateListRequest.auth = authentication;

            ShipmentServicePortService port = new ShipmentServicePortService();
            GetShipmentTemplateListResponse getShipmentTemplateListResponse = port.GetShipmentTemplateList(getShipmentTemplateListRequest);
            ShipmentApiModel[] shipmentApiModels = getShipmentTemplateListResponse.shipmentTemplates;

            return shipmentApiModels;
        }

        public Product GetProductBySellerCode(int urunID)
        {
            com.n11Product.api.Authentication authentication = new com.n11Product.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            GetProductBySellerCodeRequest getProductBySellerCodeRequest = new GetProductBySellerCodeRequest();
            getProductBySellerCodeRequest.auth = authentication;
            getProductBySellerCodeRequest.sellerCode = urunID.ToString();

            ProductServicePortService port = new ProductServicePortService();
            GetProductBySellerCodeResponse getProductBySellerCodeResponse = port.GetProductBySellerCode(getProductBySellerCodeRequest);
            Product product = getProductBySellerCodeResponse.product;

            return product;
        }

        public ProductBasic[] GetProductList()
        {
            com.n11Product.api.Authentication authentication = new com.n11Product.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            com.n11Product.api.RequestPagingData requestPagingData = new com.n11Product.api.RequestPagingData();
            requestPagingData.currentPage = 0;
            requestPagingData.pageSize = 10;

            GetProductListRequest getProductListRequest = new GetProductListRequest();
            getProductListRequest.auth = authentication;
            getProductListRequest.pagingData = requestPagingData;
            
            ProductServicePortService port = new ProductServicePortService();
            GetProductListResponse getProductListResponse = port.GetProductList(getProductListRequest);
            ProductBasic[] productBasics = getProductListResponse.products;

            return productBasics;
        }

        public SaveProductResponse SaveProduct(Urun urun)
        {
            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);
            #if DEBUG
                siteUrl = "https://aysemtuhafiye.com";
            #endif
            com.n11Product.api.Authentication authentication = new com.n11Product.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            CategoryRequest categoryRequest = new CategoryRequest();
            categoryRequest.id = urun.entegrasyonBilgi.n11KategoriID;

            ProductImage[] productImages = new ProductImage[urun.urunResimleri.Count];
            for (int i = 0; i < urun.urunResimleri.Count; i++)
            {
                ProductImage productImage = new ProductImage();
                productImage.url = siteUrl + "/Content/icerik/urun/orjinal/" + urun.urunResimleri[i].resim;
                productImage.order = urun.urunResimleri[i].sira.ToString();
                productImages[i] = productImage;
            }

            //ProductAttributeRequest[] productAttributeRequests = new ProductAttributeRequest[0];
            /*
            ProductDiscountRequest productDiscountRequest = new ProductDiscountRequest();
            productDiscountRequest.startDate = "dd/MM/yyyy";
            productDiscountRequest.endDate = "dd/MM/yyyy";
            productDiscountRequest.type = "";
            productDiscountRequest.value = "1";
            */

            ProductSkuRequest[] productSkuRequests = new ProductSkuRequest[1];
            ProductSkuRequest productSkuRequest = new ProductSkuRequest();
            productSkuRequest.quantity = urun.miktar.ToString();
            productSkuRequest.optionPrice = urun.entegrasyonBilgi.GetN11Fiyat(urun.fiyat);
            productSkuRequests[0] = productSkuRequest;

            ProductRequest productRequest = new ProductRequest();
            productRequest.productSellerCode = urun.urunID.ToString();
            productRequest.title = urun.urunAdi;
            productRequest.subtitle = urun.urunAdi;
            productRequest.description = urun.urunAciklamasi;
            //productRequest.attributes = productAttributeRequests;//Özellik ve değerleri
            productRequest.category= categoryRequest;
            productRequest.price = urun.entegrasyonBilgi.GetN11Fiyat(urun.fiyat);
            productRequest.currencyType = "TL";//USD|EUR|TL
            productRequest.images = productImages;
            //productRequest.saleStartDate = "dd/MM/yyyy";//Satışa başlama tarihi
            //productRequest.saleEndDate = "dd/MM/yyyy";//Satışı bitirme tarihi
            //productRequest.productionDate = "dd/MM/yyyy";//Üretim tarihi
            //productRequest.expirationDate = "dd/MM/yyyy";//Son kullanma tarihi
            productRequest.productCondition = "1";//1:Yeni|2:2.El
            productRequest.preparingDay = (urun.kargoSuresi < 1 ? 1 : urun.kargoSuresi).ToString();
            //productRequest.domestic = true;//Yerli ürün mü true|false
            //productRequest.discount = productDiscountRequest;//İndirim bilgisi
            productRequest.shipmentTemplate = urun.entegrasyonBilgi.n11TemplateName;
            productRequest.stockItems = productSkuRequests;
            //productRequest.groupAttribute = "";
            //productRequest.groupItemCode = "";
            //productRequest.itemName = "";
            //productRequest.maxPurchaseQuantity = "1";//Maximum satın alma miktarı

            SaveProductRequest saveProductRequest = new SaveProductRequest();
            saveProductRequest.auth = authentication;
            saveProductRequest.product = productRequest;

            ProductServicePortService port = new ProductServicePortService();
            SaveProductResponse saveProductResponse = port.SaveProduct(saveProductRequest);

            if(saveProductResponse.result.status == "success")
            {
                new EntegrasyonBilgi().N11UrunIDGuncelle(urun.urunID, saveProductResponse.product.id);
            }

            return saveProductResponse;
        }
    
        public UpdateProductBasicResponse UpdateProductBasic(Urun urun)
        {
            string siteUrl = string.Format("{0}://{1}{2}{3}", HttpContext.Current.Request.Url.Scheme, HttpContext.Current.Request.Url.Host, HttpContext.Current.Request.Url.Port == 80 ? string.Empty : ":" + HttpContext.Current.Request.Url.Port, HttpContext.Current.Request.ApplicationPath);
            #if DEBUG
                siteUrl = "https://aysemtuhafiye.com";
            #endif

            Product product = GetProductBySellerCode(urun.urunID);

            com.n11Product.api.Authentication authentication = new com.n11Product.api.Authentication
            {
                appKey = apiKey,
                appSecret = apiSecret
            };

            ProductImage[] productImages = new ProductImage[urun.urunResimleri.Count];
            for (int i = 0; i < urun.urunResimleri.Count; i++)
            {
                ProductImage productImage = new ProductImage();
                productImage.url = siteUrl + "/Content/icerik/urun/orjinal/" + urun.urunResimleri[i].resim;
                productImage.order = urun.urunResimleri[i].sira.ToString();
                productImages[i] = productImage;
            }

            ProductUpdateSkuBasicRequest[] productUpdateSkuBasicRequests = new ProductUpdateSkuBasicRequest[1];
            ProductUpdateSkuBasicRequest productUpdateSkuBasicRequest = new ProductUpdateSkuBasicRequest();
            productUpdateSkuBasicRequest.quantity = urun.miktar.ToString();
            productUpdateSkuBasicRequest.optionPrice = urun.entegrasyonBilgi.GetN11Fiyat(urun.fiyat);
            productUpdateSkuBasicRequest.id = product.stockItems.stockItem[0].id;
            productUpdateSkuBasicRequests[0] = productUpdateSkuBasicRequest;

            UpdateProductBasicRequest updateProductBasicRequest = new UpdateProductBasicRequest();
            updateProductBasicRequest.auth = authentication;
            updateProductBasicRequest.description = urun.urunAciklamasi;
            updateProductBasicRequest.images = productImages;
            updateProductBasicRequest.price = urun.entegrasyonBilgi.GetN11Fiyat(urun.fiyat);
            updateProductBasicRequest.productId = urun.entegrasyonBilgi.n11UrunID;
            //updateProductBasicRequest.productSellerCode = urun.urunID.ToString();
            updateProductBasicRequest.stockItems = productUpdateSkuBasicRequests;

            ProductServicePortService port = new ProductServicePortService();
            UpdateProductBasicResponse updateProductBasicResponse = port.UpdateProductBasic(updateProductBasicRequest);

            return updateProductBasicResponse;
        }
    }
}