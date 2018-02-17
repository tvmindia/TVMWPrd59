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
            bundles.Add(new ScriptBundle("~/bundles/userpluginjs").Include("~/Scripts/jquery.noty.packaged.min.js", "~/Scripts/custom.js", "~/Scripts/Chart.js", "~/Scripts/sweetalert.min.js", "~/Scripts/bootstrap-notify.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrapdatepicker").Include("~/Scripts/bootstrap-datepicker.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/MvcDatalist/DataList").Include("~/Scripts/MvcDatalist/mvc-datalist.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/select2").Include("~/Scripts/select2.min.js"));


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
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RawMaterial").Include("~/Scripts/UserJS/RawMaterial.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/Bank").Include("~/Scripts/UserJS/Bank.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/PurchaseOrderJS/ViewPurchaseOrder").Include("~/Scripts/UserJS/PurchaseOrderJS/ViewPurchaseOrder.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/ViewRequisition").Include("~/Scripts/UserJS/RequisitionJS/ViewRequisition.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/NewRequisition").Include("~/Scripts/UserJS/RequisitionJS/NewRequisition.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJs/RequisitionJS/RequisitionApproval").Include("~/Scripts/UserJS/RequisitionJS/RequisitionApproval.js"));
            bundles.Add(new ScriptBundle("~/bundles/UserJS/IssueToProduction/AddIssueToProduction").Include("~/Scripts/UserJS/IssueToProduction/AddIssueToProduction.js"));
        }
    }
}