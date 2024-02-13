//using CK.Models;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using OfficeOpenXml.Style;
//using OfficeOpenXml;
//using System.Diagnostics;
//using System.Globalization;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using CK.Model;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System.Linq;
//using Microsoft.IdentityModel.Tokens;


//namespace CK.Controllers
//{
//    [Authorize]
//    public class HomeController : Controller
//    {
//        //aa //mar
//        private readonly ILogger<HomeController> _logger;

//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }
//        DataCenterContext db = new DataCenterContext();

//        bool exported = false;
//        [HttpGet]
//        public IActionResult Index(string startDate, string endDate, int? Store, int? Department, int? Supplier, bool exportAfterClick, string[] selectedItems,
//               bool VPerDay, bool VPerMonYear, bool VPerMon, bool VPerYear, bool VQty, bool VPrice, bool VStoreName, bool VDepartment, bool VTotalSales
//              , bool VItemName, bool VSupplierId, bool VSupplierName,
//               int? monthToFilter, string ItemLookupCodeTxt, string ItemNameTxt, bool TMT, bool RMS, bool Yesterday)
//        {
//            DataCenterContext db = new DataCenterContext();
//            CkhelperdbContext db3 = new CkhelperdbContext();
//            db.Database.SetCommandTimeout(600);// Set the timeout in seconds
//            //Store List Text=StoreName , Value = StoreId
//            ViewBag.VBStore = db3.Liststores
//                .GroupBy(m => m.StoreName)
//                .Select(group => new { Store = group.First().StoreId, StoreName = group.Key })
//                .OrderBy(m => m.StoreName)
//                .ToList();
//            //      ViewBag.VBStore = db3.Liststores
//            //.GroupBy(m => m.StoreName)
//            //.Select(group => new
//            //{
//            //    StoreId = TMT ? group.First().StoreIdD : RMS ? group.First().StoreIdR : group.First().StoreId,
//            //    StoreName = group.Key
//            //})
//            //.OrderBy(m => m.StoreName)
//            //.ToList();
//            //ViewBag.VBStore = db3.Liststores
//            //    .GroupBy(m => m.StoreName)
//            //    .Select(group =>
//            //    {
//            //        var selectedStoreId = TMT ? group.First().StoreIdD : RMS ? group.First().StoreIdR : group.First().StoreId;

//            //        return new
//            //        {
//            //            StoreId = selectedStoreId,
//            //            StoreName = group.Key
//            //        };
//            //    })
//            //    .OrderBy(m => m.StoreName)
//            //    .ToList();
//            //Department List Text=Name , Value = Id
//            ViewBag.VBDepartment = db.Departments
//                                                 .GroupBy(m => m.Name)
//                                                 .Select(group => new { Code = group.First().Code, Name = group.Key })
//                                                 .OrderBy(m => m.Name)
//                                                 .ToList();

//            //Supplier List Text=SupplierName , Value = Code 
//            ViewBag.VBSupplier = db.Suppliers
//                                             .GroupBy(m => m.SupplierName)
//                                                 .Select(group => new { Code = group.First().Code, SupplierName = group.Key })
//                                                 .OrderBy(m => m.SupplierName)
//                                                 .ToList();

//            ViewBag.VBItemBarcode = db.Items.Select(m => new { m.Id, m.ItemLookupCode }).Distinct();
//            ViewBag.VBStoreFranchise = db.Stores
//                 .Where(m => m.Franchise != null)
//                 .Select(m => m.Franchise)
//                 .Distinct()
//                 .ToList();
//            // Views 
//            IQueryable<RptSale> RptSales = db.RptSales;
//            IQueryable<RptSalesAxt> RptSalesAxts = db.RptSalesAxts;
//            IQueryable<RptSalesAx> RptSalesAxes = db.RptSalesAxes;
//            IQueryable<RptSalesAll> RptSalesAlls = db.RptSalesAlls;

//            if (!string.IsNullOrEmpty(startDate))
//            {
//                DateTime startDateTime = Convert.ToDateTime(startDate, new CultureInfo("en-GB"));
//                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate >= startDateTime);
//                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate.HasValue && s.TransDate >= startDateTime);
//                // RptSalesAxts = RptSalesAxts.Where(s => s.Transdate.HasValue && s.Transdate >= startDateTime);
//            }

//            if (!string.IsNullOrEmpty(endDate))
//            {
//                DateTime endDateTime = Convert.ToDateTime(endDate, new CultureInfo("en-GB"));
//                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate <= endDateTime);
//                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate.HasValue && s.TransDate <= endDateTime);
//                // RptSalesAxts = RptSalesAxts.Where(s => s.Transdate.HasValue && s.Transdate <= endDateTime);
//            }   // Declared at the class level
//            DateTime currentDate = DateTime.Now;
//            DateTime lastWeekStartDate = currentDate.AddDays(-7);
//            DateTime lastMonthDate = currentDate.AddMonths(-1);
//            DateTime firstDayOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
//            DateTime lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);
//            if (Yesterday)
//            {
//                RptSales = RptSales.Where(s => s.TransDate >= firstDayOfCurrentMonth && s.TransDate <= lastDayOfLastMonth);
//                RptSalesAxts = RptSalesAxts.Where(s => s.TransDate >= firstDayOfCurrentMonth && s.TransDate <= lastDayOfLastMonth);
//            }
//            if (Store != null)
//            {
//                if (RMS)
//                {
//                    RptSales = RptSales.Where(s => s.StoreId == Store.Value);
//                }
//                if (TMT)
//                {
//                    RptSalesAxts = RptSalesAxts.Where(s => s.StoreId == Store.Value.ToString());
//                }
//                // RptSalesAxts = RptSalesAxts.Where(s => s.Store == Store.Value.ToString());

//            }

//            if (monthToFilter.HasValue)
//            {
//                RptSales = RptSales.Where(s => s.TransDate.HasValue && s.TransDate.Value.Month == monthToFilter.Value);
//            }
//            if (Department > 0)
//            {
//                RptSales = RptSales.Where(s => s.DpId == Department.Value.ToString());
//                RptSalesAxts = RptSalesAxts.Where(s => s.DpId == Department.Value.ToString());

//            }
//            if (string.IsNullOrEmpty(startDate) && string.IsNullOrEmpty(endDate) && Store is null && Department is null/* && ItemLookupCodeTxt is null*/)
//            {
//                return View();
//            }
//            if (TMT)
//            {
//                // TMT is selected, filter based on ItemID in RptSalesAxts
//                if (!string.IsNullOrEmpty(ItemLookupCodeTxt))
//                {
//                    RptSalesAxes = RptSalesAxes.Where(s => s.Itemid == ItemLookupCodeTxt);
//                }
//            }
//            else if (RMS)
//            {
//                // RMS is selected, filter based on ItemLookupCode in RptSales
//                if (!string.IsNullOrEmpty(ItemLookupCodeTxt))
//                {
//                    RptSales = RptSales.Where(s => s.ItemLookupCode.ToUpper() == ItemLookupCodeTxt.ToUpper());
//                }
//            }
//            if (RMS)
//            {
//                // TMT is selected, filter based on ItemID in RptSalesAxts
//                if (Supplier != null)
//                {
//                    RptSales = RptSales.Where(s => s.SupplierCode == Supplier.Value.ToString());
//                }
//            }
//            else
//            {
//                RptSalesAxes = RptSalesAxes.Where(s => s.SupplierCode == Supplier);

//            }

//            // Dynamic GroupBy based on selected values
//            IQueryable<dynamic> reportData1;
//            if (TMT && RMS)
//            {
//                if (VPerDay == true)
//                {
//                    if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                       .GroupBy((RptSalesAll d) => new { Date = d.TransDate.Value.Date, d.DpName, d.StoreName, d.ByMonth, d.ByYear })
//                       .Select(g => new
//                       {
//                           Total = g.Sum(d => d.TotalSales),
//                           TotalQty = g.Sum(d => d.Qty),
//                           PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                           DepName = g.Key.DpName,
//                           Price = g.Max(d => d.Price),
//                           PerMonth = g.Key.ByMonth,
//                           PerYear = g.Key.ByYear,
//                           StoreName = g.Key.StoreName,

//                       });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.DpName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                    else if (Store != null && Department == null)
//                    {
//                        reportData1 = RptSalesAlls
//                       .GroupBy((RptSalesAll d) => new { Date = d.TransDate.Value.Date, d.StoreName, d.ByMonth, d.ByYear })
//                       .Select(g => new
//                       {
//                           Total = g.Sum(d => d.TotalSales),
//                           TotalQty = g.Sum(d => d.Qty),
//                           PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                           Price = g.Max(d => d.Price),
//                           PerMonth = g.Key.ByMonth,
//                           PerYear = g.Key.ByYear,
//                           StoreName = g.Key.StoreName,

//                       });

//                    }

//                    else
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                }
//                else if (VPerMon == true)
//                {
//                    if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                           .GroupBy((RptSalesAll d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               PerMonth = g.Key.ByMonth,
//                               PerYear = g.Key.ByYear,
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName
//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.DpName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else if (Store != null && Department == null)
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.StoreName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                StoreName = g.Key.StoreName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                }
//                else
//                {
//                    if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                           .GroupBy((RptSalesAll d) => new { d.DpName, d.StoreName })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName
//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.DpName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSalesAlls
//                            .GroupBy((RptSalesAll d) => new { d.StoreName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                StoreName = g.Key.StoreName,
//                                Price = g.Max(d => d.Price),
//                            });
//                    }
//                }
//            }
//            else if (TMT)
//            {
//                if (VPerDay == true)
//                {
//                    if (Store != null && Department != null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSalesAxts
//                       .GroupBy((RptSalesAxt d) => new { Date = d.TransDate.Value.Date, d.DpName, d.StoreName, d.ByMonth, d.ByYear, d.ItemLookupCode })
//                       .Select(g => new
//                       {
//                           Total = g.Sum(d => d.TotalSales),
//                           TotalQty = g.Sum(d => d.Qty),
//                           PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                           DepName = g.Key.DpName,
//                           Price = g.Max(d => d.Price),
//                           PerMonth = g.Key.ByMonth,
//                           PerYear = g.Key.ByYear,
//                           StoreName = g.Key.StoreName,
//                           ItemLookupCodeTxt = g.Key.ItemLookupCode,
//                       });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { d.DpName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                }
//                else if (VPerMon == true)
//                {
//                    if (Store != null && Department != null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSalesAxts
//                           .GroupBy((RptSalesAxt d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear, d.ItemLookupCode })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               PerMonth = g.Key.ByMonth,
//                               PerYear = g.Key.ByYear,
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName,
//                               ItemLookupCodeTxt = g.Key.ItemLookupCode
//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { d.DpName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else if (Store != null && Department == null)
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { d.StoreName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                StoreName = g.Key.StoreName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                }
//                else
//                {
//                    if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSalesAxts
//                           .GroupBy((RptSalesAxt d) => new { d.StoreName, d.DpName })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName,
//                               DepName = g.Key.DpName,
//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { d.DpName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                            });
//                    }
//                    //else if (Store != null && Department == null && ItemLookupCodeTxt != null)
//                    //{
//                    //    reportData1 = RptSalesAxts
//                    //       .GroupBy((RptSalesAxt d) => new { d.StoreName, d.ItemLookupCode })
//                    //       .Select(g => new
//                    //       {
//                    //           Total = g.Sum(d => d.TotalSales),
//                    //           TotalQty = g.Sum(d => d.Qty),
//                    //           StoreName = g.Key.StoreName,
//                    //           ItemLookupCodeTxt = g.Key.ItemLookupCode,
//                    //       });

//                    //}
//                    else if (Store != null && Department == null && ItemLookupCodeTxt != null)
//                    {
//                        // Query RptSalesAxes for ItemLookupCodeTxt
//                        var axesData = RptSalesAxes
//                            .Where(d => d.Itemid == ItemLookupCodeTxt)
//                            .GroupBy(d => new { d.Itemid })
//                            .Select(g => new
//                            {
//                                ItemLookupCodeTxt = g.Key.Itemid,
//                                Total = (decimal?)null,   // Provide default values for Total and TotalQty
//                                TotalQty = (decimal?)null,
//                                StoreName = (string)null
//                            })
//                            .AsEnumerable(); // Convert to IEnumerable

//                        // Query RptSalesAxts for the remaining data
//                        var axtsData = RptSalesAxts
//                            .Where(d => d.StoreName == Store.ToString())
//                            .GroupBy(d => new { d.StoreName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                StoreName = g.Key.StoreName,
//                                ItemLookupCodeTxt = (string)null   // Provide a default value for ItemLookupCodeTxt
//                            })
//                            .AsEnumerable(); // Convert to IEnumerable

//                        // Convert axtsData to a common type
//                        var combinedData = axtsData.Select(g => new
//                        {
//                            Total = g.Total,
//                            TotalQty = g.TotalQty,
//                            StoreName = g.StoreName,
//                            ItemLookupCodeTxt = (string)null  // Set to null or provide a default value
//                        })
//                         .Concat(axesData.Select(g => new
//                         {
//                             Total = (decimal?)null,
//                             TotalQty = (decimal?)null,
//                             StoreName = (string)null,
//                             ItemLookupCodeTxt = g.ItemLookupCodeTxt
//                         }));

//                        // Assign to the reportData1 variable
//                        reportData1 = combinedData.AsQueryable();  // Convert back to IQueryable if needed

//                    }

//                    else if (Store != null && Department == null && ItemLookupCodeTxt == null && Supplier != null)
//                    {
//                        reportData1 = RptSalesAxts
//                         .GroupBy((RptSalesAxt d) => new { d.StoreName })
//                         .Select(g => new
//                         {
//                             Total = g.Sum(d => d.TotalSales),
//                             TotalQty = g.Sum(d => d.Qty),
//                             Price = g.Max(d => d.Price),
//                             StoreName = g.Key.StoreName,
//                             //Supplier = g.Key.SupplierName,

//                         });
//                    }
//                    else if (VStoreName && VDepartment)
//                    {
//                        reportData1 = RptSalesAxts
//                               .GroupBy((RptSalesAxt d) => new { d.DpName, d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                                   Price = g.Max(d => d.Price),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (VStoreName)
//                    {
//                        reportData1 = RptSalesAxts
//                               .GroupBy((RptSalesAxt d) => new { d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (VDepartment)
//                    {
//                        reportData1 = RptSalesAxts
//                               .GroupBy((RptSalesAxt d) => new { d.DpName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                               });
//                    }
//                    else if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSalesAxts
//                               .GroupBy((RptSalesAxt d) => new { })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                               });
//                    }
//                    else if (VStoreName && VDepartment)
//                    {
//                        reportData1 = RptSalesAxts
//                               .GroupBy((RptSalesAxt d) => new { d.DpName, d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                                   Price = g.Max(d => d.Price),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (Store == null && Department == null)
//                    {
//                        // Aggregate total sales for all departments for each store within the specified date range
//                        reportData1 = RptSalesAxts

//                            .GroupBy((RptSalesAxt d) => new { d.StoreName, d.StoreId /*Date = d.TransDate.Value.Date*/ })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                //PerMonth = g.Key.Date.ToString("yyyy-MM-dd"),
//                                StoreName = g.Key.StoreName,
//                                Id = g.Key.StoreId,
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSalesAxts
//                            .GroupBy((RptSalesAxt d) => new { })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                            });
//                    }
//                }
//            }
//            else if (RMS)
//            {
//                if (VPerDay == true)
//                {
//                    if (Store != null && Department != null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSales
//                           .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date, d.DpName, d.StoreName, d.ByMonth, d.ByYear, d.ItemLookupCode })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               PerMonth = g.Key.ByMonth,
//                               PerYear = g.Key.ByYear,
//                               StoreName = g.Key.StoreName,
//                               ItemLookupCodeTxt = g.Key.ItemLookupCode

//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.DpName, Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                    else if (Store != null && Department == null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear, d.StoreName, d.ItemLookupCode })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                StoreName = g.Key.StoreName,
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),

//                                ItemLookupCodeTxt = g.Key.ItemLookupCode,

//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),
//                            });
//                    }
//                }
//                else if (VPerMon == true)
//                {
//                    if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSales
//                           .GroupBy((RptSale d) => new { d.ByMonth, d.DpName, d.StoreName, d.ByYear })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               PerMonth = g.Key.ByMonth,
//                               PerYear = g.Key.ByYear,
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName
//                           });

//                    }
//                    else if (Store == null && Department != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.DpName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                DepName = g.Key.DpName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else if (Store != null && Department == null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.StoreName, d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                StoreName = g.Key.StoreName,
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                    else if (Store != null && Department == null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { Date = d.TransDate.Value.Date, d.ByMonth, d.ByYear, d.StoreName, d.ItemLookupCode })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                StoreName = g.Key.StoreName,
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                                PerDay = g.Key.Date.ToString("yyyy-MM-dd"),

//                                ItemLookupCodeTxt = g.Key.ItemLookupCode,

//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.ByMonth, d.ByYear })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                PerMonth = g.Key.ByMonth,
//                                PerYear = g.Key.ByYear,
//                            });
//                    }
//                }
//                else
//                {
//                    if (Store != null && Department != null && ItemLookupCodeTxt != null)
//                    {
//                        reportData1 = RptSales
//                           .GroupBy((RptSale d) => new { d.DpName, d.StoreName, d.ItemLookupCode })
//                           .Select(g => new
//                           {
//                               Total = g.Sum(d => d.TotalSales),
//                               TotalQty = g.Sum(d => d.Qty),
//                               DepName = g.Key.DpName,
//                               Price = g.Max(d => d.Price),
//                               StoreName = g.Key.StoreName,
//                               ItemLookupCodeTxt = g.Key.ItemLookupCode
//                           });
//                    }
//                    else if (Store != null && Department == null && ItemLookupCodeTxt != null && Supplier != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.StoreName, d.ItemLookupCode, d.SupplierName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                StoreName = g.Key.StoreName,
//                                ItemLookupCodeTxt = g.Key.ItemLookupCode,
//                                Supplier = g.Key.SupplierName,

//                            });
//                    }
//                    else if (Store != null && Department == null && ItemLookupCodeTxt == null && Supplier != null)
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { d.StoreName, d.SupplierName })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                StoreName = g.Key.StoreName,
//                                Supplier = g.Key.SupplierName,

//                            });
//                    }
//                    else if (VStoreName && VDepartment)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { d.DpName, d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                                   Price = g.Max(d => d.Price),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (VStoreName)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (VDepartment)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { d.DpName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                               });
//                    }
//                    else if (VSupplierName)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { d.SupplierName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   Supplier = g.Key.SupplierName,
//                               });
//                    }
//                    else if (Store != null && Department != null)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                               });
//                    }
//                    else if (VStoreName && VDepartment)
//                    {
//                        reportData1 = RptSales
//                               .GroupBy((RptSale d) => new { d.DpName, d.StoreName })
//                               .Select(g => new
//                               {
//                                   Total = g.Sum(d => d.TotalSales),
//                                   TotalQty = g.Sum(d => d.Qty),
//                                   DepName = g.Key.DpName,
//                                   Price = g.Max(d => d.Price),
//                                   StoreName = g.Key.StoreName
//                               });
//                    }
//                    else if (Store == null && Department == null)
//                    {
//                        // Aggregate total sales for all departments for each store within the specified date range
//                        reportData1 = RptSales

//                            .GroupBy((RptSale d) => new { d.StoreName, d.StoreId /*Date = d.TransDate.Value.Date*/ })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                                Price = g.Max(d => d.Price),
//                                //PerMonth = g.Key.Date.ToString("yyyy-MM-dd"),
//                                StoreName = g.Key.StoreName,
//                                Id = g.Key.StoreId,
//                            });
//                    }
//                    else
//                    {
//                        reportData1 = RptSales
//                            .GroupBy((RptSale d) => new { })
//                            .Select(g => new
//                            {
//                                Total = g.Sum(d => d.TotalSales),
//                                TotalQty = g.Sum(d => d.Qty),
//                            });
//                    }
//                }
//            }
//            // if Not RMS or TMT
//            else
//            {
//                return View();
//            }
//            ViewBag.Data = reportData1;
//            //var reportData1 = ViewBag.Data as IEnumerable<dynamic>;
//            if (exportAfterClick == false)
//            {
//                return View();
//            }
//            else
//            {
//                ViewBag.Data = reportData1;
//                Stopwatch stopwatch = new Stopwatch();
//                stopwatch.Start();
//                using (var package = new ExcelPackage())
//                {
//                    var worksheet = package.Workbook.Worksheets.Add("SalesReport");

//                    // Add header row
//                    int columnCount = 1; // Start with the first column (A)

//                    if (VPerYear == true || VPerMonYear == true)
//                        worksheet.Cells[1, columnCount++].Value = "Date Per Year";
//                    if (VPerMon == true || VPerMonYear == true)
//                        worksheet.Cells[1, columnCount++].Value = "Date Per Month";
//                    if (VPerDay == true)
//                        worksheet.Cells[1, columnCount++].Value = "Date Per Day";
//                    if (VDepartment)
//                        worksheet.Cells[1, columnCount++].Value = "Department Name";
//                    if (Store != null && VStoreName)
//                        worksheet.Cells[1, columnCount++].Value = "Store Name";
//                    if (Store == null && Department == null)
//                        worksheet.Cells[1, columnCount++].Value = "Store Name";
//                    if (ItemLookupCodeTxt != null)
//                        worksheet.Cells[1, columnCount++].Value = "item Lookup Code";
//                    if (VSupplierName)
//                        worksheet.Cells[1, columnCount++].Value = "Supplier Name";
//                    if (VQty == true)
//                        worksheet.Cells[1, columnCount++].Value = "Total Qty";
//                    if (VPrice == true)
//                        worksheet.Cells[1, columnCount++].Value = "Max Price";
//                    if (VTotalSales)
//                        worksheet.Cells[1, columnCount++].Value = "Total Sales";

//                    // Set header style
//                    using (var range = worksheet.Cells[1, 1, 1, columnCount])
//                    {
//                        range.Style.Font.Bold = true;
//                        //range.Style.Fill.PatternType = ExcelFillStyle.Solid;
//                        //range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
//                    }

//                    // Add data rows
//                    int row = 2;
//                    foreach (var item in reportData1)
//                    {
//                        columnCount = 1; // Reset column count for each row
//                        if (VPerYear == true || VPerMonYear == true)
//                            worksheet.Cells[row, columnCount++].Value = item.PerYear;
//                        if (VPerMon == true || VPerMonYear == true)
//                            worksheet.Cells[row, columnCount++].Value = item.PerMonth;
//                        if (VPerDay == true)
//                            worksheet.Cells[row, columnCount++].Value = item.PerDay;
//                        if (VDepartment)
//                            worksheet.Cells[row, columnCount++].Value = item.DepName;

//                        if (Store != null && VStoreName)
//                            worksheet.Cells[row, columnCount++].Value = item.StoreName;

//                        if (ItemLookupCodeTxt != null)
//                            worksheet.Cells[row, columnCount++].Value = item.ItemLookupCodeTxt;

//                        if (VSupplierName)
//                            worksheet.Cells[row, columnCount++].Value = item.Supplier;

//                        if (VQty == true)
//                            worksheet.Cells[row, columnCount++].Value = item.TotalQty;

//                        if (VPrice == true)
//                            worksheet.Cells[row, columnCount++].Value = item.Price;
//                        if (VTotalSales)
//                            worksheet.Cells[row, columnCount++].Value = item.Total;
//                        row++;
//                    }

//                    // Auto fit columns
//                    worksheet.Cells.AutoFitColumns();

//                    // Save the file
//                    var stream = new MemoryStream();
//                    package.SaveAs(stream);

//                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "SalesReport.xlsx");
//                }
//                stopwatch.Stop();

//                // Log or display the elapsed time
//                Console.WriteLine($"Time taken for export: {stopwatch.ElapsedMilliseconds / 1000.0} second");


//                // Return the elapsed time as JSON to the client
//                return Json(new { ElapsedTime = stopwatch.ElapsedMilliseconds / 1000.0 });

//            }

//        }
//        // Handle the case when the checkbox is not checked
//        //return Json(new { Message = "Export canceled. Checkbox not checked





//        //[HttpGet]
//        //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//        //public async Task<IActionResult> Logout()
//        //{
//        //    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

//        //    // Set TempData variable to indicate logout
//        //    TempData["IsLoggedOut"] = true;

//        //    // Clear session on logout
//        //    HttpContext.Session.Clear();

//        //    // Prevent caching by setting appropriate HTTP headers
//        //    if (!Response.Headers.ContainsKey("Cache-Control"))
//        //    {
//        //        Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
//        //    }

//        //    return RedirectToAction("Login", "Login");
//        //}
//        [HttpGet]
//        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
//        public async Task<IActionResult> LogOut()
//        {
//            // Sign out the user
//            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

//            // Set a TempData variable to indicate logout
//            TempData["IsLoggedOut"] = true;

//            // Clear session on logout
//            HttpContext.Session.Clear();

//            // Prevent caching by setting appropriate HTTP headers
//            //Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
//            //Response.Headers.Add("Pragma", "no-cache");
//            //Response.Headers.Add("Expires", "0");
//            try
//            {
//                if (!Response.Headers.ContainsKey("Cache-Control"))
//                {
//                    Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
//                }

//                if (!Response.Headers.ContainsKey("Pragma"))
//                {
//                    Response.Headers.Add("Pragma", "no-cache");
//                }

//                if (!Response.Headers.ContainsKey("Expires"))
//                {
//                    Response.Headers.Add("Expires", "0");
//                }

//                return RedirectToAction("Login", "Login");
//            }

//            catch (Exception ex)
//            {
//                Console.WriteLine($"Exception in LogOut action: {ex.Message}");
//                return RedirectToAction("Login", "Login");
//            }
//        }


//        public IActionResult Privacy()
//        {
//            return View();
//        }

//        public IActionResult index1()
//        {
//            return View();
//        }


//        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
//        public IActionResult Error()
//        {
//            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
//        }
//    }
//}
