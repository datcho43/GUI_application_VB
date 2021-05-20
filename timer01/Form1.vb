Public Class Form1
    Public S_num As Integer
    Public M_num As Integer
    Public Sub num()
        If M_num = 10 Then
            M_num = 0
            S_num += 1
        End If
        lblNum1.Text = $"{S_num}.{M_num}"
        M_num += 1
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        num()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Timer1.Enabled = False Then
            Timer1.Start()
            Button1.Text = "停止"
        Else

            Timer1.Stop()
            Button1.Text = "開始"
        End If



    End Sub
End Class
