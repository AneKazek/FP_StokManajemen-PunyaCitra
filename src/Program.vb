Imports System.Windows.Forms

Module Program
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        DatabaseHelper.InitializeDatabase()
        Application.Run(New MainForm())
    End Sub
End Module
