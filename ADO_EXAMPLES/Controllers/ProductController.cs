using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ADO_EXAMPLES.Data;
using ADO_EXAMPLES.Models;

namespace ADO_EXAMPLES.Controllers
{
    public class ProductController : Controller
    {
        Product_Data _productdata = new Product_Data();
        // GET: Product
        public ActionResult Index()
        {
            var ProductList =_productdata.GetAllProducts();

            if(ProductList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently Products not available in the Database.";
            }

            return View(ProductList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product =_productdata.GetProductByID(id).FirstOrDefault();

            if(product ==null)
            {
                TempData["InfoMessage"] = "product not available with id";
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product, Product_Data Product_Data)
        {
            bool IsInserted = false;
            try
            {
                // TODO: Add insert logic here
                if(ModelState.IsValid) 
                {
                   IsInserted = Product_Data.InsertProduct(product);

                    if(IsInserted) 
                    {
                        TempData["SuccessMessage"] = "Product details saved successfully";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Unable to save";
                    }
                }
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var products= _productdata.GetProductByID(id).FirstOrDefault();

            if(products == null)
            {
                TempData["InfoMessage"] = "Product not available  with ID" + id.ToString();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Product/Edit/5
        [HttpPost,ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isUpdated = _productdata.UpdateProduct(product);
                    if (isUpdated)
                    {
                        TempData["SuccessMessage"] = "Product details updated";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Not updated";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
            
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {

            try
            {
                var product = _productdata.GetProductByID(id).FirstOrDefault();
                if (product == null)
                {
                    TempData["InfoMessage"] = "Product not available  with ID" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // POST: Product/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                String result = _productdata.DeleteProduct(id);

                if (result.Contains("deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;

                }
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {

                TempData["ErrorMessage"] =ex.Message;
                return View();
            }
        }
    }
}
