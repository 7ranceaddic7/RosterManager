﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RosterManager
{
    internal static class WindowContractDispute
    {
        // This class creates a GUI for ContractDispute data and processes user inputs for this data.        
        #region ContractDisput Window (GUI)
        internal static float windowWidth = 700;
        internal static float windowHeight = 330;
        internal static Rect Position = new Rect(0, 0, 700, 330);
        internal static bool ShowWindow = false;
        internal static bool ToolTipActive = false;
        internal static bool ShowToolTips = true;
        internal static string ToolTip = "";
        private static Vector2 ScrollViewerPosition = Vector2.zero;
        private static bool recontractFlag = false;   

        internal static void Display(int windowId)
        {            
            // Reset Tooltip active flag...            
            ToolTipActive = false;

            Rect rect = new Rect(680, 4, 16, 16);
            if (GUI.Button(rect, new GUIContent("", "Close Window")))
            {
                ToolTip = "";
                WindowContractDispute.ShowWindow = false;
            }
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 0, 0);

            
            GUILayout.BeginVertical();
            ScrollViewerPosition = GUILayout.BeginScrollView(ScrollViewerPosition, GUILayout.Height(280), GUILayout.Width(680));
            GUILayout.BeginVertical();

            DisplayDisputes();
                       
            GUILayout.EndVertical();
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            List<RMKerbal> disputekerbals = new List<RMKerbal>(RMLifeSpan.Instance.rmKerbals.ALLRMKerbals.Values.Where(a => a.salaryContractDispute == true).ToList());
            string buttonToolTip = string.Empty;
            if (disputekerbals.Count() > 0)
            {
                buttonToolTip = "Accept All Payrises. Kerbals will continue working.";
                if (GUILayout.Button(new GUIContent("Accept All", buttonToolTip), RMStyle.ButtonStyle))
                {
                    // Accept all disputes.                
                    foreach (RMKerbal disputekerbal in disputekerbals)
                    {
                        if (!disputekerbal.salaryContractDisputeProcessed)
                            AcceptDispute(disputekerbal);
                    }
                    ToolTip = "";
                    WindowContractDispute.ShowWindow = false;
                }
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "Decline All Payrises, All listed kerbals will become tourists until you can pay them.";
                if (GUILayout.Button(new GUIContent("Decline All", buttonToolTip), RMStyle.ButtonStyle))
                {
                    // Decline all disputes.                    
                    foreach (RMKerbal disputekerbal in disputekerbals)
                    {
                        if (!disputekerbal.salaryContractDisputeProcessed)
                            DeclineDispute(disputekerbal);
                    }
                    ToolTip = "";
                    WindowContractDispute.ShowWindow = false;

                }
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);
            }
            if (recontractFlag)
            {
                buttonToolTip = "ReContract All Disputed Kerbals.";
                if (GUILayout.Button(new GUIContent("Recontract All", buttonToolTip), RMStyle.ButtonStyle))
                {
                    // ReContract all disputes.                    
                    foreach (RMKerbal disputekerbal in disputekerbals)
                    {
                        if (disputekerbal.salaryContractDisputeProcessed)
                            ReContract(disputekerbal);
                    }
                    ToolTip = "";
                    WindowContractDispute.ShowWindow = false;

                }
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);
            }            

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            GUI.DragWindow(new Rect(0, 0, Screen.width, 30));
            RMSettings.RepositionWindows("Contract Disputes");
        }
              
        private static void DisplayDisputes()
        {
            GUILayout.BeginHorizontal();
            Rect rect = new Rect();
            GUI.enabled = true;
            string buttonToolTip = string.Empty;
            buttonToolTip = "Crew Member Name.";
            GUILayout.Label(new GUIContent("Crew Name", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(130));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            buttonToolTip = "Crew Member's Current Salary.";
            GUILayout.Label(new GUIContent("Salary", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(80));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            buttonToolTip = "Crew Member's Current Outstanding/Owing Salary.";
            GUILayout.Label(new GUIContent("BackPay", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(80));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            buttonToolTip = "How many Pay periods salary has been in dispute.";
            GUILayout.Label(new GUIContent("#", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(30));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            buttonToolTip = "Payrise Requested.";
            GUILayout.Label(new GUIContent("PayRise Amt", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(90));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            buttonToolTip = "kerbals usual profession when working.";
            GUILayout.Label(new GUIContent("Profession", buttonToolTip), RMStyle.LabelStyleBold, GUILayout.Width(75));
            rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

            GUILayout.EndHorizontal();
            recontractFlag = false;
            List<RMKerbal> disputekerbals = new List<RMKerbal>(RMLifeSpan.Instance.rmKerbals.ALLRMKerbals.Values.Where(a => a.salaryContractDispute == true).ToList());
            if (disputekerbals.Count() == 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("There are no Contracts currently in Dispute.", RMStyle.LabelStyle, GUILayout.Width(280));
                GUILayout.EndHorizontal();
            }
            foreach (RMKerbal disputekerbal in disputekerbals)
            {
                GUIStyle labelStyle = null;
                if (disputekerbal.status == ProtoCrewMember.RosterStatus.Dead || disputekerbal.status == ProtoCrewMember.RosterStatus.Missing
                    || (disputekerbal.salaryContractDispute && disputekerbal.Trait == "Tourist"))
                    labelStyle = RMStyle.LabelStyleRed;
                else if (disputekerbal.salaryContractDispute && disputekerbal.Trait != "Tourist")
                    labelStyle = RMStyle.LabelStyleMagenta;
                else if (disputekerbal.status == ProtoCrewMember.RosterStatus.Assigned)
                    labelStyle = RMStyle.LabelStyleYellow;
                else
                    labelStyle = RMStyle.LabelStyle;

                buttonToolTip = string.Empty;
                GUILayout.BeginHorizontal();
                buttonToolTip = "Crew Member Name.";
                GUILayout.Label(disputekerbal.Name, labelStyle, GUILayout.Width(130));
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "Crew Member's Current Salary.";
                GUILayout.Label(disputekerbal.salary.ToString("###,##0"), labelStyle, GUILayout.Width(80));
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "Crew Member's Current Outstanding/Owing Salary.";
                GUILayout.Label(disputekerbal.owedSalary.ToString("###,##0"), labelStyle, GUILayout.Width(80));
                rect = GUILayoutUtility.GetLastRect();
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "How many Pay periods salary has been in dispute.";
                GUILayout.Label(disputekerbal.salaryContractDisputePeriods.ToString(), labelStyle, GUILayout.Width(30));
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "Payrise Requested.";
                GUILayout.Label(disputekerbal.payriseRequired.ToString(), labelStyle, GUILayout.Width(90));
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                buttonToolTip = "Kerbals usual Profession.";
                GUILayout.Label(disputekerbal.RealTrait, labelStyle, GUILayout.Width(75));
                if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                    ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                if (disputekerbal.salaryContractDisputeProcessed || disputekerbal.Trait == "Tourist")
                {
                    recontractFlag = true;
                    if (Funding.CanAfford((float)disputekerbal.salary + (float)disputekerbal.owedSalary))
                    {
                        GUI.enabled = true;
                        buttonToolTip = "Re-Instate kerbal and pay all owed funds.";
                    }
                    else
                    {
                        GUI.enabled = false;
                        buttonToolTip = "Not enough funds to Re-Instate kerbal and pay all owed funds.";
                    }

                    if (GUILayout.Button(new GUIContent("Re-Contract", buttonToolTip), RMStyle.ButtonStyle, GUILayout.Width(100)))
                    {
                        ReContract(disputekerbal);                                           
                    }
                    rect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                        ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);
                    GUI.enabled = true;
                }
                else
                {
                    buttonToolTip = "Accept Payrise.";
                    if (GUILayout.Button(new GUIContent("Accept", buttonToolTip), RMStyle.ButtonStyle, GUILayout.Width(80)))
                    {
                        //Accept payrise
                        AcceptDispute(disputekerbal);
                    }
                    rect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                        ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);

                    buttonToolTip = "Decline Payrise. Kerbal will become a tourist until paid.";
                    if (GUILayout.Button(new GUIContent("Decline", buttonToolTip), RMStyle.ButtonStyle, GUILayout.Width(80)))
                    {
                        //Decline payrise
                        DeclineDispute(disputekerbal);
                    }
                    rect = GUILayoutUtility.GetLastRect();
                    if (Event.current.type == EventType.Repaint && ShowToolTips == true)
                        ToolTip = Utilities.SetActiveTooltip(rect, Position, GUI.tooltip, ref ToolTipActive, 30, 5 - ScrollViewerPosition.y);
                }
                
                GUILayout.EndHorizontal();
            }
        }

        private static void DeclineDispute(RMKerbal disputekerbal)
        {
            KeyValuePair<string, RMKerbal> kerbal = new KeyValuePair<string, RMKerbal>(disputekerbal.Name, disputekerbal);
            RMLifeSpanAddon.Instance.resignKerbal(disputekerbal.Kerbal, kerbal);
            disputekerbal.salaryContractDisputeProcessed = true;            
        }

        private static void AcceptDispute(RMKerbal disputekerbal)
        {
            disputekerbal.salary += disputekerbal.payriseRequired;
            disputekerbal.salaryContractDispute = true;
            // Calculate and store their backpay.
            if (RMLifeSpan.Instance.rmGameSettings.SalaryPeriodisYearly)
            {
                disputekerbal.owedSalary += disputekerbal.salary * 12;
            }
            else
            {
                disputekerbal.owedSalary += disputekerbal.salary;
            }
            disputekerbal.salaryContractDisputeProcessed = true;
            disputekerbal.payriseRequired = 0;
        }

        private static void ReContract(RMKerbal disputekerbal)
        {
            if (Funding.CanAfford((float)disputekerbal.salary + (float)disputekerbal.owedSalary))
            {
                Funding.Instance.AddFunds(-(disputekerbal.salary + disputekerbal.owedSalary), TransactionReasons.CrewRecruited);
                disputekerbal.timelastsalary = Planetarium.GetUniversalTime();
                disputekerbal.salaryContractDispute = false;
                disputekerbal.salaryContractDisputeProcessed = true;
                disputekerbal.salaryContractDisputePeriods = 0;
                disputekerbal.payriseRequired = 0d;
                disputekerbal.owedSalary = 0d;
                //If they are a tourist (dispute) and not dead (DeepFreeze frozen/comatose) set them back to crew                   
                if (disputekerbal.type == ProtoCrewMember.KerbalType.Tourist && disputekerbal.status != ProtoCrewMember.RosterStatus.Dead)
                {
                    disputekerbal.type = ProtoCrewMember.KerbalType.Crew;
                    disputekerbal.Kerbal.type = ProtoCrewMember.KerbalType.Crew;                    
                    disputekerbal.Trait = disputekerbal.RealTrait;
                    disputekerbal.Kerbal.trait = disputekerbal.RealTrait;
                    KerbalRoster.SetExperienceTrait(disputekerbal.Kerbal, disputekerbal.Kerbal.trait);
                    RMKerbal.RegisterExperienceTrait(disputekerbal);
                }
            }
        }

        #endregion ContractDisput Window (GUI)
    }
}