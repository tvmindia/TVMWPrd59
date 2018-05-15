using System.Web.Optimization;

namespace ProductionApp.UserInterface.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/Content/boot").Include("~/Content/bootstrap.css", "~/Content/bootstrap-theme.css", "~/Content/font-awesome.min.css", "~/Content/Custom.css", "~/Content/sweetalert.css"));
            bundles.Add(new StyleBundle("~/Content/AdminLTE/css/plugins").Include("~/Content/AdminLTE/css/jvectormap/jquery-jvectormap-1.2.2.css", "~/Content/AdminLTE/css/AdminLTE.min.css", "~/Content/AdminLTE/css/skins/_all-skins.min.css"));
            //bundles.Add(new StyleBundle("~/AdminLTE/bootstrap/css/plugins").Include("~/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css", "~/AdminLTE/dist/css/AdminLTE.min.css", "~/AdminLTE/dist/css/skins/_all-skins.min.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrapdatepicker").Include("~/Content/bootstrap-datepicker3.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatable").Include("~/Content/DataTables/css/dataTables.bootstrap.min.css", "~/Content/DataTables/css/responsive.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatablecheckbox").Include("~/Content/DataTables/css/dataTables.checkboxes.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableSelect").Include("~/Content/DataTables/css/select.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableButtons").Include("~/Content/DataTables/css/buttons.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedColumns").Include("~/Content/DataTables/css/fixedColumns.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/DataTables/css/datatableFixedHeader").Include("~/Content/DataTables/css/fixedHeader.dataTables.min.css"));
            bundles.Add(new StyleBundle("~/Content/MvcDatalist/Datalist").Include("~/Content/MvcDatalist/mvc-datalist.css"));
            bundles.Add(new StyleBundle("~/Content/css/select2").Include("~/Content/css/select2.min.css"));

            //-------------------
            bundles.Add(new StyleBundle("~/Content/UserCSS/Login").Include("~/Content/UserCSS/Login.css"));
            bundles.Add(new StyleBundle("~/Content/css/Select2").Include("~/Content/css/select2.css"));
            bundles.Add(new StyleBundle("~/Content/css/Selectmin").Include("~/Content/css/select2.min.css"));
            bundles.Add(new ScriptBundle("~/Content/UserCSS/BillOfMaterial").Include("~/Content/UserCSS/BillOfMaterial.css"));
            bundles.Add(new StyleBundle("~/Content/css/jquery-ui").Include("~/Content/css/jquery-ui.css"));

            //---------------------
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-3.1.1.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryform").Include("~/Scripts/jquery.form.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include("~/Scripts/AdminLTE/fastclick.min.js", "~/Scripts/AdminLTE/adminlte.min.js", "~/Scripts/AdminLTE/jquery.sparkline.min.js", "~/Scripts/AdminLTE/jquery.slimscroll.min.js", "~/Scripts/AdminLTE/Chart.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/AdminLTEDash").Include("~/Scripts/AdminLTE/dashboard2.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryunobtrusiveajaxvalidate").Include("~/Scripts/jquery.validate.min.js", "~/Scripts/jquery.validate.unobtrusive.min.js", "~/Scripts/jquery.unobtrusive-ajax.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatable").Include("~/Scripts/DataTables/jquery.dataTables.min.js", "~/Scripts/DataTables/dataTables.bootstrap.min.js", "~/Scripts/DataTables/dataTables.responsive.min.js", "~/Scripts/DataTables/responsive.bootstrap.min.js", "~/Scripts/DataTables/dataTables.fixedHeader.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableSelect").Include("~/Scripts/DataTables/dataTables.select.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatablecheckbox").Include("~/Scripts/DataTables/dataTables.checkboxes.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableButtons").Include("~/Scripts/DataTables/dataTables.buttons.min.js", "~/Scripts/DataTables/buttons.flash.min.js", "~/Scripts/DataTables/buttons.html5.min.js", "~/Scripts/DataTables/buttons.print.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/datatableFixedColumns").Include("~/Scripts/DataTables/dataTables.fixedColumns.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/jsZip").Include("~/Scripts/jszip.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/Custom.js", "~/Scripts/UserJS/Master.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js", "~/Scripts/bootstrap-notify.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/MvcDatalist/DataList").Include("~/Scripts/MvcDatalist/mvc-datalist.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/selectmin").Include("~/Scripts/select2.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include("~/Scripts/jquery-ui.js"));
            bundles.Add(new ScriptBundle("~/bundles/pdf").Include("~/Scripts/PDF.js"));

            //----------------------
            bundles.Add(new ScriptBundle("~/bundles/ManageAccess").Include("~/Scripts/UserJS/ManageAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/ManageSubObjectAccess").Include("~/Scripts/UserJS/ManageSubObjectAccess.js"));
            bundles.Add(new ScriptBundle("~/bundles/Login").Include("~/Scripts/UserJS/Login.js"));
            bundles.Add(new ScriptBundle("~/bundles/User").Include("~/Scripts/UserJS/User.js"));
            bundles.Add(new ScriptBundle("~/bundles/Privileges").Include("~/Scripts/UserJS/Privileges.js"));
            bundles.Add(new ScriptBundle("~/bundles/PrivilegesView").Include("~/Scripts/UserJS/PrivilegesView.js"));
            bundles.Add(new ScriptBundle("~/bundles/Application").Include("~/Scripts/UserJS/Application.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppObject").Include("~/Scripts/UserJS/AppObject.js"));
            bundles.Add(new ScriptBundle("~/bundles/AppSubobject").Include("~/Scripts/UserJS/AppSubobject.js"));
            bundles.Add(new ScriptBundle("~/bundles/Roles").Include("~/Scripts/UserJS/Roles.js"));

            //---------------------------------
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Material").Include("~/Scripts/UserJS/Material.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/ChartOfAccount").Include("~/Scripts/UserJS/ChartOfAccount.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/ProductCategory").Include("~/Scripts/UserJS/ProductCategory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Product").Include("~/Scripts/UserJS/Product.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Customer").Include("~/Scripts/UserJS/Customer.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerJS/ViewCustomer").Include("~/Scripts/UserJS/CustomerJS/ViewCustomer.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerJS/NewCustomer").Include("~/Scripts/UserJS/CustomerJS/NewCustomer.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierJS/ViewSupplier").Include("~/Scripts/UserJS/SupplierJS/ViewSupplier.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierJS/NewSupplier").Include("~/Scripts/UserJS/SupplierJS/NewSupplier.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Bank").Include("~/Scripts/UserJS/Bank.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Approver").Include("~/Scripts/UserJS/Approver.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SubComponent").Include("~/Scripts/UserJS/SubComponent.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Stage").Include("~/Scripts/UserJS/Stage.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/PurchaseOrderJS/ViewPurchaseOrder").Include("~/Scripts/UserJS/PurchaseOrderJS/ViewPurchaseOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/PurchaseOrderJS/NewPurchaseOrder").Include("~/Scripts/UserJS/PurchaseOrderJS/NewPurchaseOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/ViewRequisition").Include("~/Scripts/UserJS/RequisitionJS/ViewRequisition.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/NewRequisition").Include("~/Scripts/UserJS/RequisitionJS/NewRequisition.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/RequisitionApproval").Include("~/Scripts/UserJS/RequisitionJS/RequisitionApproval.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/IssueToProduction/AddIssueToProduction").Include("~/Scripts/UserJS/IssueToProduction/AddIssueToProduction.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/IssueToProduction/ListIssueToProduction").Include("~/Scripts/UserJS/IssueToProduction/ListIssueToProduction.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReceiptJS/ViewMaterialReceipt").Include("~/Scripts/UserJS/MaterialReceiptJS/ViewMaterialReceipt.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReceiptJS/NewMaterialReceipt").Include("~/Scripts/UserJS/MaterialReceiptJS/NewMaterialReceipt.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ApprovalHistory").Include("~/Scripts/UserJS/DocumentApproval/ApprovalHistory.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ApproveDocument").Include("~/Scripts/UserJS/DocumentApproval/ApproveDocument.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/DocumentSummary").Include("~/Scripts/UserJS/DocumentApproval/DocumentSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ViewPendingDocuments").Include("~/Scripts/UserJS/DocumentApproval/ViewPendingDocuments.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Home").Include("~/Scripts/UserJS/Home/SalesSummary.js", "~/Scripts/UserJS/Home/PurchaseSummary.js", "~/Scripts/UserJS/Home/IncomeExpenseSummary.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/MaterialStockAdjustment/ListStockAdjustment").Include("~/Scripts/UserJS/MaterialStockAdjustment/ListStockAdjustment.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Home").Include("~/Scripts/UserJS/Home/SalesSummary.js", "~/Scripts/UserJS/Home/PurchaseSummary.js", "~/Scripts/UserJS/Home/IncomeExpenseSummary.js", "~/Scripts/UserJS/Home/FinishedGoodSummary.js", "~/Scripts/UserJS/Home/MaterialSummary.js", "~/Scripts/UserJS/Home/ProductionSummary.js"));//
            bundles.Add(new ScriptBundle("~/bundles/UserJS/MaterialStockAdjustment/NewStockAdjustment").Include("~/Scripts/UserJS/MaterialStockAdjustment/NewStockAdjustment.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SalesOrder/ListSalesOrder").Include("~/Scripts/UserJS/SalesOrder/ListSalesOrder.js"));//
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SalesOrder/AddSalesOrder").Include("~/Scripts/UserJS/SalesOrder/AddSalesOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/BillOfMaterial/ViewBillOfMaterial").Include("~/Scripts/UserJs/BillOfMaterialJS/ViewBillOfMaterial.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/BillOfMaterial/NewBillOfMaterial").Include("~/Scripts/UserJs/BillOfMaterialJS/NewBillOfMaterial.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReturnFromProductionJS/ViewRecieveFromProduction").Include("~/Scripts/UserJS/MaterialReturnFromProductionJS/ViewRecieveFromProduction.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReturnFromProductionJS/NewRecieveFromProduction").Include("~/Scripts/UserJS/MaterialReturnFromProductionJS/NewRecieveFromProduction.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/PackingSlip/AddPackingSlip").Include("~/Scripts/UserJS/PackingSlip/AddPackingSlip.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/PackingSlip/ListPackingSlips").Include("~/Scripts/UserJS/PackingSlip/ListPackingSlips.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/FinishedGoodStockAdj/ViewFinishedGoodStockAdj").Include("~/Scripts/UserJS/FinishedGoodStockAdj/ViewFinishedGoodStockAdj.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/FinishedGoodStockAdj/NewFinishedGoodStockAdj").Include("~/Scripts/UserJS/FinishedGoodStockAdj/NewFinishedGoodStockAdj.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerPayment/NewCustomerPayment").Include("~/Scripts/UserJS/CustomerPayment/NewCustomerPayment.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerPayment/ViewCustomerPayment").Include("~/Scripts/UserJS/CustomerPayment/ViewCustomerPayment.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Employee").Include("~/Scripts/UserJS/Employee.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Assembly/ViewAssembly").Include("~/Scripts/UserJS/Assembly/ViewAssembly.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Assembly/NewAssembly").Include("~/Scripts/UserJS/Assembly/NewAssembly.js"));
            //CustomerInvoice 
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerInvoice/NewCustomerInvoice").Include("~/Scripts/UserJS/CustomerInvoice/NewCustomerInvoice.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/CustomerInvoice/ViewCustomerInvoice").Include("~/Scripts/UserJS/CustomerInvoice/ViewCustomerInvoice.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReturn/NewMaterialReturn").Include("~/Scripts/UserJS/MaterialReturn/NewMaterialReturn.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/MaterialReturn/ViewMaterialReturn").Include("~/Scripts/UserJS/MaterialReturn/ViewMaterialReturn.js"));
            //ProductionTracking
            bundles.Add(new ScriptBundle("~/bundles/UserJs/ProductionTracking/NewProductionTracking").Include("~/Scripts/UserJS/ProductionTracking/NewProductionTracking.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/ProductionTracking/ViewProductionTracking").Include("~/Scripts/UserJS/ProductionTracking/ViewProductionTracking.js"));
            //SupplierInvoice 
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierInvoice/NewSupplierInvoice").Include("~/Scripts/UserJS/SupplierInvoice/NewSupplierInvoice.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierInvoice/ViewSupplierInvoice").Include("~/Scripts/UserJS/SupplierInvoice/ViewSupplierInvoice.js"));
            //SupplierPayment 
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierPayment/NewSupplierPayment").Include("~/Scripts/UserJS/SupplierPayment/NewSupplierPayment.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/SupplierPayment/ViewSupplierPayment").Include("~/Scripts/UserJS/SupplierPayment/ViewSupplierPayment.js"));
            //DocumentApproval-ApprovalHistory
            bundles.Add(new ScriptBundle("~/bundles/UserJs/DocumentApproval/ViewApprovalHistory").Include("~/Scripts/UserJS/DocumentApproval/ViewApprovalHistory.js"));
            //Report
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/Report").Include("~/Scripts/UserJS/Report/Report.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/RequisitionSummaryReport").Include("~/Scripts/UserJS/Report/RequisitionSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/RequisitionDetailReport").Include("~/Scripts/UserJS/Report/RequisitionDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PurchaseSummaryReport").Include("~/Scripts/UserJS/Report/PurchaseSummaryReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PurchaseDetailReport").Include("~/Scripts/UserJS/Report/PurchaseDetailReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/PurchaseRegisterReport").Include("~/Scripts/UserJS/Report/PurchaseRegisterReport.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Report/InventoryReOrderStatusReport").Include("~/Scripts/UserJS/Report/InventoryReOrderStatusReport.js"));
            //OtherIncome
            bundles.Add(new ScriptBundle("~/bundles/UserJs/OtherIncome/ViewOtherIncome").Include("~/Scripts/UserJS/OtherIncome/ViewOtherIncome.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/OtherIncome/NewOtherIncome").Include("~/Scripts/UserJS/OtherIncome/NewOtherIncome.js"));
            //OtherExpense
            bundles.Add(new ScriptBundle("~/bundles/UserJs/OtherExpense/ViewOtherExpense").Include("~/Scripts/UserJS/OtherExpense/ViewOtherExpense.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/OtherExpense/NewOtherExpense").Include("~/Scripts/UserJS/OtherExpense/NewOtherExpense.js"));
            
        }
    }
}