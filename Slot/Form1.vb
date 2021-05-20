Public Class Form1
    Private slot_num As Integer
    Private count As Integer = 0
    Public Function now_count(num) As Integer
        Return num
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Select Case count
            Case 0
                Timer1.Start()
                Button1.Text = "右リール停止"
                count += 1
            Case 1
                Button1.Text = "中リール停止"
                lblNum3.Text = now_count(slot_num)
                count += 1
            Case 2
                Button1.Text = "左リール停止"
                lblNum2.Text = now_count(slot_num)
                count += 1
            Case 3
                lblNum1.Text = now_count(slot_num)
                count += 1
                Timer1.Stop()
                Button1.Text = "開始"
                count = 0

        End Select
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If slot_num = 10 Then
            slot_num = 0
        End If
        slot_num += 1
        Select Case count
            Case 0

            Case 1
                lblNum1.Text = slot_num
                lblNum2.Text = slot_num + 1
                lblNum3.Text = slot_num + 2
            Case 2
                lblNum1.Text = slot_num
                lblNum2.Text = slot_num + 1
            Case 3
                lblNum2.Text = slot_num + 1

        End Select

    End Sub
End Class
