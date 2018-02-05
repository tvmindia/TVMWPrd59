using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProductionApp.UserInterface.Models;
using SAMTool.DataAccessObject.DTO;
using System.Reflection;

namespace ProductionApp.UserInterface.SecurityFilter
{
    public class ToolBarAccess
    {
        public ToolboxViewModel SetToolbarAccess(ToolboxViewModel toolbar,Permission _permission) {
            try
            {

                if (_permission.SubPermissionList != null)
                {
                    toolbar.addbtn = setAccess(toolbar.addbtn, _permission);
                    toolbar.backbtn = setAccess(toolbar.backbtn, _permission);
                    toolbar.calculateBtn = setAccess(toolbar.calculateBtn, _permission);
                    toolbar.ClearBtn = setAccess(toolbar.ClearBtn, _permission);
                    toolbar.CloseBtn = setAccess(toolbar.CloseBtn, _permission);
                    toolbar.deletebtn = setAccess(toolbar.deletebtn, _permission);
                    toolbar.DepositBtn = setAccess(toolbar.DepositBtn, _permission);
                    toolbar.NotyBtn = setAccess(toolbar.NotyBtn, _permission);
                    toolbar.PayBtn = setAccess(toolbar.PayBtn, _permission);
                    toolbar.PrintBtn = setAccess(toolbar.PrintBtn, _permission);
                    toolbar.resetbtn = setAccess(toolbar.resetbtn, _permission);
                    toolbar.returnBtn = setAccess(toolbar.returnBtn, _permission);
                    toolbar.savebtn = setAccess(toolbar.savebtn, _permission);
                    toolbar.WithdrawBtn = setAccess(toolbar.WithdrawBtn, _permission);
                    toolbar.LimitBtn = setAccess(toolbar.LimitBtn, _permission);
                    toolbar.AssignBtn = setAccess(toolbar.AssignBtn, _permission);
                    toolbar.HistoryBtn = setAccess(toolbar.HistoryBtn, _permission);
                }

                return toolbar;
            }
            catch (Exception)
            {

                return toolbar;
            }

        }

        private ToolBoxStructure setAccess(ToolBoxStructure btn, Permission _permission) {

 
            if (_permission.SubPermissionList.Exists(s => s.Name == btn.SecurityObject) ==false || _permission.SubPermissionList.First(s => s.Name == btn.SecurityObject).AccessCode.Contains("R"))
            {
                btn.HasAccess = true;

            }
            else
            {
                btn.HasAccess = false;
                btn.DisableReason = "Access Denied";
            }

            return btn;
        }

    }
}