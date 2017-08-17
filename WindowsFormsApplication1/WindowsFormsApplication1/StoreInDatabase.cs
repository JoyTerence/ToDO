using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace namespaceToDO
{
    class StoreInDatabase
    {
        String connectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\barnes4\Documents\Visual Studio 2013\Projects\WindowsFormsApplication1\WindowsFormsApplication1\Database1.mdf;Integrated Security=True";

        public void Insert(int ID, String line, String lock_state, bool checked_state)
        {
            int lock_state_bit = 0;
            int checked_state_bit = 0;
            if(lock_state == "locked")
                lock_state_bit = 1;
            if(checked_state == true)
                checked_state_bit  = 1;

            String query = "UPDATE db SET Text=@line, Lock=@lock_state_bit, checkBox= @checked_state_bit WHERE ID=@ID";
            SqlDataAdapter da = new SqlDataAdapter();   
            using(var cn = new SqlConnection(connectionString))
            using(var cmd = new SqlCommand(query, cn))
            {
                da.UpdateCommand = cmd;
                da.UpdateCommand.Parameters.AddWithValue("@ID", ID);
                da.UpdateCommand.Parameters.AddWithValue("@line", line);
                da.UpdateCommand.Parameters.AddWithValue("@lock_state_bit", lock_state_bit);
                da.UpdateCommand.Parameters.AddWithValue("@checked_state_bit", checked_state_bit);
                cn.Open();
                da.UpdateCommand.ExecuteNonQuery();
                cn.Close();
            }
        }

        public String[] Retrieve(int row)
        {
            String[] singleRow = new String[3];
            SqlDataReader myReader = null;
            String query = "select * from db WHERE ID=@row";
            using(var cn = new SqlConnection(connectionString))
            using(var cmd = new SqlCommand(query, cn))
            {
                cn.Open();
                cmd.Parameters.AddWithValue("@row", row);
                myReader = cmd.ExecuteReader();
                while (myReader.Read())
                {
                    singleRow[0] = myReader["Text"].ToString();
                    singleRow[1] = myReader["lock"].ToString();
                    singleRow[2] = myReader["checkBox"].ToString();
                }
                cn.Close();
            }
            return singleRow;
        }
    }
}
