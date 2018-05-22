using ProductionApp.UserInterface.Models;
using ProductionApp.DataAccessObject.DTO;
using SAMTool.DataAccessObject.DTO;

namespace ProductionApp.UserInterface.App_Start
{
    public class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //domain <===== viewmodel
                //viewmodel =====> domain
                //ReverseMap() makes it possible to map both ways.


                //*****SAMTOOL MODELS 
                config.CreateMap<LoginViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<UserViewModel, SAMTool.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<SysMenuViewModel, SAMTool.DataAccessObject.DTO.SysMenu>().ReverseMap();
                config.CreateMap<RolesViewModel, SAMTool.DataAccessObject.DTO.Roles>().ReverseMap();
                config.CreateMap<ApplicationViewModel, SAMTool.DataAccessObject.DTO.Application>().ReverseMap();
                config.CreateMap<AppObjectViewModel, SAMTool.DataAccessObject.DTO.AppObject>().ReverseMap();
                config.CreateMap<AppSubobjectViewmodel, SAMTool.DataAccessObject.DTO.AppSubobject>().ReverseMap();
                config.CreateMap<CommonViewModel, SAMTool.DataAccessObject.DTO.Common>().ReverseMap();
                config.CreateMap<ManageAccessViewModel, SAMTool.DataAccessObject.DTO.ManageAccess>().ReverseMap();
                config.CreateMap<ManageSubObjectAccessViewModel, SAMTool.DataAccessObject.DTO.ManageSubObjectAccess>().ReverseMap();
                config.CreateMap<PrivilegesViewModel, SAMTool.DataAccessObject.DTO.Privileges>().ReverseMap();


                //****SAMTOOL MODELS END

                //****PRODUCTION APP MODELS
                config.CreateMap<AMCSysModuleViewModel, AMCSysModule>().ReverseMap();
                config.CreateMap<AMCSysMenuViewModel, AMCSysMenu>().ReverseMap();
                config.CreateMap<DataTablePagingViewModel, DataTablePaging>().ReverseMap();
                config.CreateMap<CommonViewModel, DataAccessObject.DTO.Common>().ReverseMap();

                config.CreateMap<BankViewModel, Bank>().ReverseMap();
                config.CreateMap<UserViewModel, ProductionApp.DataAccessObject.DTO.User>().ReverseMap();
                config.CreateMap<BankAdvanceSearchViewModel, BankAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialIssueViewModel, MaterialIssue>().ReverseMap();
                config.CreateMap<MaterialIssueDetailViewModel, MaterialIssueDetail>().ReverseMap();
                config.CreateMap<MaterialViewModel, Material>().ReverseMap();
                config.CreateMap<MaterialAdvanceSearchViewModel, MaterialAdvanceSearch>().ReverseMap();
                config.CreateMap<ProductCategoryViewModel, ProductCategory>().ReverseMap();
                config.CreateMap<ProductCategoryAdvanceSearchViewModel, ProductCategoryAdvanceSearch>().ReverseMap();
                config.CreateMap<UnitViewModel, Unit>().ReverseMap();
                config.CreateMap<DocumentTypeViewModel, DocumentType>().ReverseMap();
                config.CreateMap<MaterialTypeViewModel, MaterialType>().ReverseMap();
                config.CreateMap<ProductViewModel, Product>().ReverseMap();
                config.CreateMap<ProductAdvanceSearchViewModel, ProductAdvanceSearch>().ReverseMap();
                config.CreateMap<ChartOfAccountViewModel, ChartOfAccount>().ReverseMap();
                config.CreateMap<ChartOfAccountAdvanceSearchViewModel, ChartOfAccountAdvanceSearch>().ReverseMap();
                config.CreateMap<ApproverViewModel, Approver>().ReverseMap();
                config.CreateMap<ApproverAdvanceSearchViewModel, ApproverAdvanceSearch>().ReverseMap();
                config.CreateMap<StageViewModel, Stage>().ReverseMap();
                config.CreateMap<StageAdvanceSearchViewModel, StageAdvanceSearch>().ReverseMap();
                config.CreateMap<SubComponentViewModel, SubComponent>().ReverseMap();
                config.CreateMap<SubComponentAdvanceSearchViewModel, SubComponentAdvanceSearch>().ReverseMap();

                config.CreateMap<PurchaseOrderViewModel, PurchaseOrder>().ReverseMap();
                config.CreateMap<PurchaseOrderAdvanceSearchViewModel, PurchaseOrderAdvanceSearch>().ReverseMap();
                config.CreateMap<PurchaseOrderDetailViewModel, PurchaseOrderDetail>().ReverseMap();

                config.CreateMap<SupplierViewModel, Supplier>().ReverseMap();

                config.CreateMap<RequisitionViewModel, Requisition>().ReverseMap();
                config.CreateMap<RequisitionDetailViewModel, RequisitionDetail>().ReverseMap();
                config.CreateMap<RequisitionAdvanceSearchViewModel, RequisitionAdvanceSearch>().ReverseMap();
                config.CreateMap<EmployeeViewModel, Employee>().ReverseMap();
                config.CreateMap<EmployeeAdvanceSearchViewModel, EmployeeAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialStockAdjViewModel, MaterialStockAdj>().ReverseMap();
                config.CreateMap<MaterialStockAdjAdvanceSearchViewModel, MaterialStockAdjAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialReceiptViewModel, MaterialReceipt>().ReverseMap();
                config.CreateMap<MaterialReceiptAdvanceSearchViewModel, MaterialReceiptAdvanceSearch>().ReverseMap();
                config.CreateMap<MaterialIssueAdvanceSearchViewModel, MaterialIssueAdvanceSearch>().ReverseMap();
                config.CreateMap<DocumentApprovalViewModel, DocumentApproval>().ReverseMap();
                config.CreateMap<DocumentApprovalAdvanceSearchViewModel, DocumentApprovalAdvanceSearch>().ReverseMap();
                config.CreateMap<SalesSummaryViewModel, SalesSummary>().ReverseMap();
                config.CreateMap<PurchaseSummaryViewModel, PurchaseSummary>().ReverseMap();
                config.CreateMap<IncomeExpenseSummaryViewModel, IncomeExpenseSummary>().ReverseMap();
                config.CreateMap<DocumentApproverViewModel, DocumentApprover>().ReverseMap();
                config.CreateMap<FinishedGoodSummaryViewModel,FinishedGoodSummary>().ReverseMap();
                config.CreateMap<MaterialSummaryViewModel, MaterialSummary>().ReverseMap();
                config.CreateMap<ProductionSummaryViewModel, ProductionSummary>().ReverseMap();
                config.CreateMap<BillOfMaterialViewModel,BillOfMaterial>().ReverseMap();
                config.CreateMap<BillOfMaterialDetailViewModel,BillOfMaterialDetail>().ReverseMap();
                config.CreateMap<BillOfMaterialAdvanceSearchViewModel,BillOfMaterialAdvanceSearch>().ReverseMap();
                config.CreateMap<BOMComponentLineViewModel, BOMComponentLine>().ReverseMap();
                config.CreateMap<BOMComponentLineStageDetailViewModel, BOMComponentLineStageDetail>().ReverseMap();
                config.CreateMap<BOMComponentLineStageViewModel, BOMComponentLineStage>().ReverseMap();
                config.CreateMap<MaterialReturnFromProductionViewModel, MaterialReturnFromProduction>().ReverseMap();
                config.CreateMap<MaterialReturnFromProductionDetailViewModel, MaterialReturnFromProductionDetail>().ReverseMap();
                config.CreateMap<CustomerViewModel, Customer>().ReverseMap();
                config.CreateMap<TaxType, TaxTypeViewModel>().ReverseMap();
                config.CreateMap<MaterialReturnFromProductionAdvanceSearchViewModel, MaterialReturnFromProductionAdvanceSearch>().ReverseMap();
                config.CreateMap<PackingSlipViewModel, PackingSlip>().ReverseMap();
                config.CreateMap<PackingSlipDetailViewModel, PackingSlipDetail>().ReverseMap();
                config.CreateMap<SalesOrderViewModel, SalesOrder>().ReverseMap();
                config.CreateMap<FinishedGoodStockAdjAdvanceSearchViewModel, FinishedGoodStockAdjAdvanceSearch>().ReverseMap();
                config.CreateMap<FinishedGoodStockAdjViewModel, FinishedGoodStockAdj>().ReverseMap();
                config.CreateMap<FinishedGoodStockAdjDetailViewModel, FinishedGoodStockAdjDetail>().ReverseMap();
                config.CreateMap<SalesOrderDetailViewModel, SalesOrderDetail>().ReverseMap();
                config.CreateMap<MaterialReturnViewModel, MaterialReturn>().ReverseMap();
                config.CreateMap<MaterialReturnDetailViewModel, MaterialReturnDetail>().ReverseMap(); 
                config.CreateMap<MaterialReturnAdvanceSearchViewModel, MaterialReturnAdvanceSearch>().ReverseMap();
                config.CreateMap<CustomerPaymentViewModel, CustomerPayment>().ReverseMap();
                config.CreateMap<CustomerPaymentDetailViewModel, CustomerPaymentDetail>().ReverseMap(); 
                config.CreateMap<CustomerPaymentAdvanceSearchViewModel, CustomerPaymentAdvanceSearch>().ReverseMap();
                config.CreateMap<DepartmentViewModel, Department>().ReverseMap(); 
                config.CreateMap<EmployeeCategoryViewModel, EmployeeCategory>().ReverseMap(); 
                config.CreateMap<AssemblyViewModel, Assembly>().ReverseMap();
                config.CreateMap<AssemblyAdvanceSearchViewModel, AssemblyAdvanceSearch>().ReverseMap();
                //CustomerInvoice 
                config.CreateMap<CustomerInvoiceViewModel,CustomerInvoice>().ReverseMap();
                config.CreateMap<CustomerInvoiceDetailViewModel, CustomerInvoiceDetail>().ReverseMap();
                config.CreateMap<CustomerInvoiceDetailLinkViewModel, CustomerInvoiceDetailLink>().ReverseMap();
                config.CreateMap<CustomerInvoiceAdvanceSearchViewModel, CustomerInvoiceAdvanceSearch>().ReverseMap();
                //PaymentTerm
                config.CreateMap<PaymentTermViewModel, PaymentTerm>().ReverseMap();
                //ProductionTracking
                config.CreateMap<ProductionTrackingViewModel, ProductionTracking>().ReverseMap();
                config.CreateMap<ProductionTrackingAdvanceSearchViewModel, ProductionTrackingAdvanceSearch>().ReverseMap();
                //Supplier Invoice 
                config.CreateMap<SupplierInvoiceViewModel, SupplierInvoice>().ReverseMap();
                config.CreateMap<SupplierInvoiceDetailViewModel, SupplierInvoiceDetail>().ReverseMap();
                config.CreateMap<SupplierInvoiceAdvanceSearchViewModel, SupplierInvoiceAdvanceSearch>().ReverseMap();
                //Supplier Payment
                config.CreateMap<SupplierPaymentViewModel, SupplierPayment>().ReverseMap();
                config.CreateMap<SupplierPaymentDetailViewModel, SupplierPaymentDetail>().ReverseMap();
                config.CreateMap<SupplierPaymentAdvanceSearchViewModel, SupplierPaymentAdvanceSearch>().ReverseMap();

                config.CreateMap<MastersCountViewModel, MastersCount>().ReverseMap();
                //Other Income
                config.CreateMap<OtherIncomeViewModel, OtherIncome>().ReverseMap();
                config.CreateMap<OtherIncomeAdvanceSearchViewModel, OtherIncomeAdvanceSearch>().ReverseMap();
                //OtherExpense
                config.CreateMap<OtherExpenseViewModel, OtherExpense>().ReverseMap();
                config.CreateMap<OtherExpenseAdvanceSearchViewModel, OtherExpenseAdvanceSearch>().ReverseMap();
                //Report
                config.CreateMap<AMCSysReportViewModel, AMCSysReport>().ReverseMap();
               
                config.CreateMap<RequisitionDetailReportViewModel, RequisitionDetailReport>().ReverseMap();

                config.CreateMap<RequisitionSummaryReportViewModel, RequisitionSummaryReport>().ReverseMap();
                config.CreateMap<PurchaseSummaryReportViewModel, PurchaseSummaryReport>().ReverseMap();
                config.CreateMap<PurchaseDetailReportViewModel, PurchaseDetailReport>().ReverseMap();
                config.CreateMap<PurchaseRegisterReportViewModel, PurchaseRegisterReport>().ReverseMap();
                config.CreateMap<InventoryReorderStatusReportViewModel, InventoryReorderStatusReport>().ReverseMap();
                config.CreateMap<StockRegisterReportViewModel, StockRegisterReport>().ReverseMap();
                config.CreateMap<BOMTreeViewModel, BOMTree>().ReverseMap();
                config.CreateMap<StockLedgerReportViewModel, StockLedgerReport>().ReverseMap();
                config.CreateMap<InventoryReOrderStatusFGReportViewModel, InventoryReOrderStatusFGReport>().ReverseMap();
                //Approval Status
                config.CreateMap<ApprovalStatusViewModel, ApprovalStatus>().ReverseMap();

            });
        }
    }
}