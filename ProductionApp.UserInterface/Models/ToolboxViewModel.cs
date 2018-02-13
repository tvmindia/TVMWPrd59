using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ProductionApp.UserInterface.Models
{
    public class ToolboxViewModel
    {
        public ToolBoxStructure backbtn;
        public ToolBoxStructure addbtn;
        public ToolBoxStructure savebtn;
        public ToolBoxStructure deletebtn;
        public ToolBoxStructure resetbtn;
        public ToolBoxStructure returnBtn;
        public ToolBoxStructure calculateBtn;
        public ToolBoxStructure CloseBtn;
        public ToolBoxStructure PrintBtn;
        public ToolBoxStructure DepositBtn;
        public ToolBoxStructure WithdrawBtn;
        public ToolBoxStructure ClearBtn;
        public ToolBoxStructure NotyBtn;
        public ToolBoxStructure PayBtn;
        public ToolBoxStructure TransferBtn;
        public ToolBoxStructure LimitBtn;
        public ToolBoxStructure ClearOutBtn;      
        public ToolBoxStructure downloadBtn;
        public ToolBoxStructure AssignBtn;
        public ToolBoxStructure HistoryBtn;
        public ToolBoxStructure ListBtn;

        public ToolboxViewModel()
        {
            backbtn.SecurityObject = "ButtonBack";
            addbtn.SecurityObject = "ButtonAdd";
            savebtn.SecurityObject = "ButtonSave";
            deletebtn.SecurityObject = "ButtonDelete";
            resetbtn.SecurityObject = "ButtonReset";
            returnBtn.SecurityObject = "ButtonReturn";
            calculateBtn.SecurityObject = "ButtonCalculate";
            CloseBtn.SecurityObject = "ButtonClose";
            PrintBtn.SecurityObject = "ButtonPrint";
            DepositBtn.SecurityObject = "ButtonDeposit";
            WithdrawBtn.SecurityObject = "ButtonWithdraw";
            ClearBtn.SecurityObject = "ButtonClear";
            NotyBtn.SecurityObject = "ButtonNoty";
            PayBtn.SecurityObject = "ButtonPay";
            TransferBtn.SecurityObject = "ButtonCashTransfer";
            LimitBtn.SecurityObject = "ButtonLimit";
            downloadBtn.SecurityObject = "ButtonDownload";
            AssignBtn.SecurityObject = "ButtonAssign";
            HistoryBtn.SecurityObject = "ButtonHistory";

            ClearOutBtn.SecurityObject = "ButtonClear";

            backbtn.HasAccess = true;
            addbtn.HasAccess = true;
            savebtn.HasAccess = true;
            deletebtn.HasAccess = true;
            resetbtn.HasAccess = true;
            returnBtn.HasAccess = true;
            calculateBtn.HasAccess = true;
            CloseBtn.HasAccess = true;
            PrintBtn.HasAccess = true;
            DepositBtn.HasAccess = true;
            WithdrawBtn.HasAccess = true;
            ClearBtn.HasAccess = true;
            NotyBtn.HasAccess = true;
            PayBtn.HasAccess = true;
            TransferBtn.HasAccess = true;
            LimitBtn.HasAccess = true;
            ClearOutBtn.HasAccess = true;
            LimitBtn.HasAccess = true;
            downloadBtn.HasAccess = true;
            AssignBtn.HasAccess = true;
            HistoryBtn.HasAccess = true;
        }

    }

    public struct ToolBoxStructure
    {
        public string Event { get; set; }
        public string Title { get; set; }//tooltip
        public string Text { get; set; }
        public string DisableReason { get; set; }
        public bool Visible { get; set; }
        public bool Disable { get; set; }
        public bool HasAccess { get; set; }
        public string SecurityObject { get; set; }
        public string Href { get; set; }
        public static explicit operator ToolBoxStructure(PropertyInfo v)
        {
            throw new NotImplementedException();
        }
    }

    public class ToolBox
    {
        public string Dom { get; set; }
        public string Action { get; set; }
        public string ViewModel { get; set; }
    }

    
}