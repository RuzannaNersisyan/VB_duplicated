'USEUNIT Library_Common
'USEUNIT Library_Contracts
'USEUNIT Library_CheckDB  
'USEUNIT Library_Colour
'USEUNIT OLAP_Library
'USEUNIT Constants
'USEUNIT Card_Library
Option Explicit

'Test Case ID 187352

Dim sDate, eDate, userName, exportOLAP, passord, dbQA, calcDate
Dim majorClient, minorClient, conjuction, clients(1), fileName

Sub Accountanty_14_Conjuction_Test() 
    Call Test_Inintialize()

    ' Կատարում է ստուգում, եթե նման անունով ֆայլ կա տրված թղթապանակում, ջնջում է    
    If aqFile.Exists(Project.Path & "Stores|Actual_OLAP16600_14.xls") Then        
        aqFileSystem.DeleteFile(Project.Path & "Stores|Actual_OLAP16600_14.xls")    
    End If 
           
    ' Արտահանել BAZEL CONTRACTS-ը    
    Call Export_To_Olap("BAZEL CONTRACTS")        
    
    ' Արտահանել SS14-ը    
    Call Export_To_Olap("SS14")        
    
    ' Արտահանել COA1-ը    
    Call Export_To_Olap("COA1")        
    
    ' Արտահանել COA2-ը    
    Call Export_To_Olap("COA2")        
    
    ' Արտահանել AtmInd-ը    
    Call Export_To_Olap("AtmInd")
    
    ' Բացել Excel-ը
    Call Initialize_Excel ()
    Call Sys.Process("EXCEL").Window("XLMAIN", "Excel", 1).Window("FullpageUIHost").Window("NetUIHWND").Click(505, 236)
    
    ' Ավելացնել AddIn թաբը
    Call AddOLAPAddIn ()
 
    ' Կատարել Աշխատանքի սկիզբ
    Call Start_Work(userName ,passord, dbQA)
    
    ' Բացել հաշվետվության ձևանմուշ տվյալների պահոց
    Call Open_Accountanty(0, 14)

    ' Հաշվարկել հաշվետվությունտ 
    Call Calculate_Report_Range(True, "", calcDate, calcDate, 1, "", "", "Հայերեն", 1, 60, "16600_14.xls")
    
    ' Անցնել sheet1
    Set fileName = Sys.Process("EXCEL").Window("XLMAIN", "16600_14.xls  [Compatibility Mode] - Excel", 1)
    fileName.Keys("^[PageUp]" & "^[PageUp]")
    
    '  Ստուգել, որ Խոշոր փոխառու հաճախորդը գտնվել է
    Log.Message "Ստուգել, որ Խոշոր փոխառու հաճախորդը գտնվել է", "", pmNormal, DivideColor
    If Excel_Find_Word(fileName, majorClient.ClientsCode) Then
        Log.Message "Major client " & majorClient.ClientsCode & " was found.", "", pmNormal, MessageColor
    Else 
        Log.Error "Major client " & majorClient.ClientsCode & " wasn't found.", "", pmNormal, ErrorColor
    End If
    
    ' Ստուգել, որ կապակցված ակտիվ չունեցող հաճախորդը առկա չէ 14 հաշվետվությունում
    Log.Message "Ստուգել, որ կապակցված ակտիվ չունեցող հաճախորդը առկա չէ 14 հաշվետվությունում", "", pmNormal, DivideColor
    If Not Excel_Find_Word(fileName, minorClient) Then
        Log.Message "Client without actives " & minorClient & " wasn't found.", "", pmNormal, MessageColor
    Else 
        Log.Error "Client without actives " & minorClient & " was found.", "", pmNormal, ErrorColor
    End If
    
    ' Փակել EXCEL-ները
    Call CloseAllExcelFiles() 
    
    ' Համակարգ մուտք գործել ADMIN օգտագործողով    
    Log.Message "Համակարգ մուտք գործել ADMIN օգտագործողով", , pmNormal, DivideColor    
    Call Test_StartUp()
    
    ' Անցում կատարել Գլխավոր հաշվապահի ԱՇՏ
    Call ChangeWorkspace(c_ChiefAcc)  
    
    ' Մուտք գործել Հաճախորդներ թղթապանակ
    Call wTreeView.DblClickItem("|¶ÉË³íáñ Ñ³ßí³å³ÑÇ ²Þî|Ð³×³Ëáñ¹Ý»ñ")
    Call Fill_Clients(majorClient)
    
    ' Կապակցել ակտիվ չունեցող հաճախորդ խոշոր փոխառուի հետ 
    Log.Message "Կապակցել ակտիվ չունեցող հաճախորդ խոշոր փոխառուի հետ ", "", pmNormal, DivideColor
    Call Cilent_Conjunction(conjuction)
    
    ' Կատարել SQL ստուգում Հաճախորդների կապակցումից հետո
    Log.Message "Կատարել SQL ստուգում Հաճախորդների կապակցումից հետո", "", pmNormal, DivideColor
    Call CheckDB_Conjuction()
    
    ' Փակել Հաճախորդներ թղթապանակը
    Call Close_Window(wMDIClient, "frmPttel")
    
    ' Փակել ՀԾ - Բանկ համակարգգը
    Call Close_AsBank()
    
    ' Բացել Excel-ը
    Call Initialize_Excel ()
    Call Sys.Process("EXCEL").Window("XLMAIN", "Excel", 1).Window("FullpageUIHost").Window("NetUIHWND").Click(505, 236)
    
    ' Ավելացնել AddIn թաբը
    Call AddOLAPAddIn ()
 
    ' Կատարել Աշխատանքի սկիզբ
    Call Start_Work(userName ,passord, dbQA)
    
    ' Բացել հաշվետվության ձևանմուշ տվյալների պահոց
    Call Open_Accountanty(0, 14)

    ' Հաշվարկել հաշվետվությունտ 
    Call Calculate_Report_Range(True, "", calcDate, calcDate, 1, "", "", "Հայերեն", 1, 60, "16600_14.xls")
    
    ' Անցնել sheet1
    Set fileName = Sys.Process("EXCEL").Window("XLMAIN", "16600_14.xls  [Compatibility Mode] - Excel", 1)
    fileName.Keys("^[PageUp]" & "^[PageUp]")
    
    '  Ստուգել, որ Խոշոր փոխառու հաճախորդը գտնվել է
    Log.Message "Ստուգել, որ Խոշոր փոխառու հաճախորդը գտնվել է", "", pmNormal, DivideColor
    If Excel_Find_Word(fileName, majorClient.ClientsCode) Then
        Log.Message "Major client " & majorClient.ClientsCode & " was found.", "", pmNormal, MessageColor
    Else 
        Log.Error "Major client " & majorClient.ClientsCode & " wasn't found.", "", pmNormal, ErrorColor
    End If
    
    ' Ստուգել, որ կապակցված ակտիվ չունեցող հաճախորդը առկա չէ 14 հաշվետվությունում
    Log.Message "Ստուգել, որ կապակցված ակտիվ չունեցող հաճախորդը առկա չէ 14 հաշվետվությունում", "", pmNormal, DivideColor
    If Not Excel_Find_Word(fileName, minorClient) Then
        Log.Message "Client without actives " & minorClient & " wasn't found.", "", pmNormal, MessageColor
    Else 
        Log.Error "Client without actives " & minorClient & " was found.", "", pmNormal, ErrorColor
    End If
    
    ' Փակել EXCEL-ները
    Call CloseAllExcelFiles() 
    
    ' Համակարգ մուտք գործել ADMIN օգտագործողով    
    Log.Message "Համակարգ մուտք գործել ADMIN օգտագործողով", , pmNormal, DivideColor    
    Call Test_StartUp()
    
    ' Անցում կատարել Գլխավոր հաշվապահի ԱՇՏ
    Call ChangeWorkspace(c_ChiefAcc) 
    
    ' Մուտք գործել Հաճախորդներ թղթապանակ
    Call wTreeView.DblClickItem("|¶ÉË³íáñ Ñ³ßí³å³ÑÇ ²Þî|Ð³×³Ëáñ¹Ý»ñ")
    Call Fill_Clients(majorClient)
    
    ' Ջնջել հաճախորդների կապակցումը
    Log.Message "Ջնջել հաճախորդների կապակցումը", "", pmNormal, DivideColor
    Call Delete_Conjunction(1, "frmPttel_2", conjuction.comment(0), "Ð³ëï³ï»ù ÷³ëï³ÃÕÃÇ çÝç»ÉÁ")
    
    ' Կատարել SQL ստուգում Հաճախորդների կապակցումը ջնջելուց հետո
    Log.Message "Կատարել SQL ստուգում Հաճախորդների կապակցումը ջնջելուց հետո", "", pmNormal, DivideColor
    Call CheckDB_Delete_Conjuction()
    
    ' Փակել Հաճախորդներ թղթապանակը
    Call Close_Window(wMDIClient, "frmPttel")
      
    ' Փակել ՀԾ - Բանկ համակարգգը
    Call Close_AsBank()
End Sub

Sub Test_StartUp()
    ' Փակել EXCEL-ները
    Call CloseAllExcelFiles() 
				Call Initialize_AsBankQA(sDate, eDate) 
				Login("ADMIN")
				' Մուտք OLAP ադմինիստրատորի ԱՇՏ
				Call ChangeWorkspace(c_OLAPAdmin)
End Sub

Sub Export_To_Olap(exportedGroup)    
    ' Համակարգ մուտք գործել ADMIN օգտագործողով    
    Log.Message "Համակարգ մուտք գործել ADMIN օգտագործողով", , pmNormal, DivideColor    
    Call Test_StartUp()        
    
    ' Մուտք գործել OLAP խմբերի տեղեկատու թղթապանակ    
    wTreeView.DblClickItem("|OLAP ³¹ÙÇÝÇëïñ³ïáñÇ ²Þî|OLAP ËÙµ»ñÇ ï»Õ»Ï³ïáõ")    
    If wMDIClient.WaitVBObject("frmPttel", 2000).Exists Then        
        ' Արտահանել         
        Log.Message "Արտահանել " & exportedGroup & " -ը", , pmNormal, DivideColor        
        If SearchInPttel("frmPttel", 0, exportedGroup) Then             
            Call OLAP_Group_Export(exportOLAP)            
            ' Փակել OLAP խմբերի տեղեկատու թղթապանակը            
            Call Close_Window(wMDIClient, "frmPttel")            
            p1.Terminate()        
        Else            
            Log.Error "Can't find searched row with " & exportedGroup & " value.", , pmNormal, ErrorColor        
        End If    
    Else         
        Log.Error "Can't open frmPttel window.", , pmNormal, ErrorColor    
    End If
End Sub

Sub Test_Inintialize()
				sDate = "20030101"
				eDate = "20250101"
    
    userName = "ADMIN" 
    passord= ""
    dbQA = "bankTesting_QA"
    
    Set majorClient = New_Clients()
    majorClient.ClientsCode = "00006296"
    
    minorClient = "00006803"
    clients(0) = "00006803"
    
    calcDate = aqConvert.DateTimeToFormatStr(aqDateTime.Now(), "%d%m%Y")
		
    Set exportOLAP = New_OLAP_Export()
    With exportOLAP
        .dateStart = aqConvert.DateTimeToFormatStr(aqDateTime.Now(), "%d%m%y")
        .dateEnd = aqConvert.DateTimeToFormatStr(aqDateTime.Now(), "%d%m%y")
        .exportOLAP = 1
    End With 
    
    Set conjuction = New_Conjuction(1)
    With conjuction
        .client(0) = "00006803"
        .name(0) = "Ð³×³Ëáñ¹ 00006803"
        .conjuctType(0) = "102"
        .conjuctName(0) = "öáËÏ³å³Ïóí³Í ÁÝÏ»ñáõÃÛáõÝ"
        .comment(0) = "²ÏïÇí ãáõÝ»óáÕ Ñ³×³Ëáñ¹"
    End With
End Sub

Sub CheckDB_Conjuction()
    Dim i, dbo_FOLDERS(3)
    
    Log.Message "fISN = " & conjuction.fIsn, "", pmNormal, MessageColor
    
    'SQL Ստուգում DOCLOG աղուսյակում համար
    Log.Message "SQL Ստուգում DOCLOG աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCLOG", "fISN", conjuction.fIsn, 2)
    Call CheckDB_DOCLOG(conjuction.fIsn, "253", "N", "1", "", 1)
    Call CheckDB_DOCLOG(conjuction.fIsn, "253", "C", "2", "", 1)
  
    'SQL Ստուգում DOCS աղուսյակում 
    Log.Message "SQL Ստուգում DOCS աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCS", "fISN", conjuction.fIsn, 1)
    Call CheckDB_DOCS(conjuction.fIsn, "CliRel", "2", "%CLICODE:00006296%", 1)
    
    'SQL Ստուգում DOCSG աղուսյակում 
    Log.Message "SQL Ստուգում DOCSG աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCSG", "fISN", conjuction.fIsn, 3)
    Call CheckDB_DOCSG(conjuction.fIsn, "RELCLIENTS", "0", "CLICODE", "00006803", 1)
    Call CheckDB_DOCSG(conjuction.fIsn, "RELCLIENTS", "0", "COMMENT", "²ÏïÇí ãáõÝ»óáÕ Ñ³×³Ëáñ¹                                                                                                                     ", 1)
    Call CheckDB_DOCSG(conjuction.fIsn, "RELCLIENTS", "0", "RELTYPE", "102", 1)
  
    'SQL Ստուգում FOLDERS աղուսյակում
    For i = 0 To 2
        Set dbo_FOLDERS(i) = New_DB_FOLDERS()
        With dbo_FOLDERS(i)
            .fISN = conjuction.fIsn
            .fNAME = "CliRel"
            .fSTATUS = "1"
            .fCOM = "Î³å³Ïóí³Í Ñ³×³Ëáñ¹Ý»ñ"
        End With 
    Next
    With dbo_FOLDERS(0)
        .fKEY = "00006296407584625"
        .fFOLDERID = "C.407584625"
        .fSPEC = "Ø»ÏÝ. - "
        .fECOM = "Related Clients"
    End With 
    With dbo_FOLDERS(1)
        .fKEY = "00006296"
        .fFOLDERID = "CliRelCode"
    End With 
    With dbo_FOLDERS(2)
        .fKEY = "00006296/00006803"
        .fFOLDERID = "CliRels"
        .fSPEC = "102²ÏïÇí ãáõÝ»óáÕ Ñ³×³Ëáñ¹"
    End With 
    Log.Message "SQL Ստուգում FOLDERS աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("FOLDERS", "fISN", conjuction.fIsn, 3)
    For i = 0 To 2
        Call CheckDB_FOLDERS(dbo_FOLDERS(i), 1)
    Next
End Sub

Sub CheckDB_Delete_Conjuction()
    Dim dbo_FOLDERS
    
    'SQL Ստուգում DOCLOG աղուսյակում համար
    Log.Message "SQL Ստուգում DOCLOG աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCLOG", "fISN", conjuction.fIsn, 3)
    Call CheckDB_DOCLOG(conjuction.fIsn, "253", "D", "999", "", 1)
    
    'SQL Ստուգում DOCS աղուսյակում 
    Log.Message "SQL Ստուգում DOCS աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCS", "fISN", conjuction.fIsn, 1)
    Call CheckDB_DOCS(conjuction.fIsn, "CliRel", "999", "%CLICODE:00006296%", 1)
    
    'SQL Ստուգում DOCSG աղուսյակում 
    Log.Message "SQL Ստուգում DOCSG աղուսյակում", "", pmNormal, SqlDivideColor
    Call CheckQueryRowCount("DOCSG", "fISN", conjuction.fIsn, 3)
    
    'SQL Ստուգում FOLDERS աղուսյակում 
    Log.Message "SQL Ստուգում FOLDERS աղուսյակում", "", pmNormal, SqlDivideColor
    Set dbo_FOLDERS = New_DB_FOLDERS()
    With dbo_FOLDERS
        .fISN = conjuction.fIsn
        .fKEY = conjuction.fIsn
        .fNAME = "CliRel"
        .fSTATUS = "0"
        .fFOLDERID = ".R." & aqConvert.DateTimeToFormatStr(aqDateTime.Now(), "%Y%m%d")
        .fSPEC = Left_Align(Get_Compname_DOCLOG(conjuction.fIsn), 16) & "GlavBux ADMIN                         002  "
        .fCOM = ""
        .fECOM = ""
    End With 
    Call CheckQueryRowCount("FOLDERS", "fISN", conjuction.fIsn, 1)
    Call CheckDB_FOLDERS(dbo_FOLDERS, 1)
End Sub