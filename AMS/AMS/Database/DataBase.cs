using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AMS.Database
{
    public class DataBase
    {
        private static string m_Path = @"D:\QMS_DB";
        private OleDbConnection m_Connector; 
        public void Database()
        {
            m_Connector = new OleDbConnection();
        }

        public OleDbConnection GetConnection()
        {
            return m_Connector;
        }

        public void ConnectDatebase()
        {
            try
            {
                m_Connector = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + m_Path + ".mdb");
                m_Connector.Open();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Unable to Connect Database");
            }
        }

        public void DisconnectDataBase()
        {
            try
            {
                if (m_Connector != null)
                    m_Connector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to DisConnect Database");
            }
        }

        public void RequestSQL(string sql)
        {
            try
            {
                ConnectDatebase();
                OleDbCommand command = new OleDbCommand(sql, m_Connector);
                command.ExecuteNonQuery();
                DisconnectDataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Request SQL : " + ex.Message);
            }
        }

        public void RequestSQL(OleDbCommand cmd)
        {
            try
            {
                ConnectDatebase();
                OleDbCommand command = cmd;
                command.Connection = m_Connector;
                command.ExecuteNonQuery();
                DisconnectDataBase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Request SQL : " + ex.Message);
            }
        }

        public DataTable RequestData(string sql)
        {
            try
            {
                ConnectDatebase();
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, m_Connector);
                DataTable data = new DataTable();
                adapter.Fill(data);
                DisconnectDataBase();

                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to Request Data From Database : " + ex.Message);
            }
            return null;
        }
    }
}
