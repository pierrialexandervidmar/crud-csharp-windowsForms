using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Crud
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static string strCon = "server=localhost;uid=root;database=condominio";
        MySqlConnection con = new MySqlConnection(strCon);
        private string strSql = string.Empty;

        private void tsbSalvar_Click(object sender, EventArgs e)
        {
            if (CamposEstaoPreenchidos())
            {
                strSql = "insert into Funcionarios (Nome, Endereco, CEP, Bairro, Cidade, UF, Telefone) values (@Nome, @Endereco, @CEP, @Bairro, @Cidade, @UF, @Telefone)";
                var comando = new MySqlCommand(strSql, con);

                comando.Parameters.Add("@Nome", MySqlDbType.VarString).Value = txtNome.Text;
                comando.Parameters.Add("@Endereco", MySqlDbType.VarString).Value = txtEndereco.Text;
                comando.Parameters.Add("@CEP", MySqlDbType.VarString).Value = mskCep.Text;
                comando.Parameters.Add("@Bairro", MySqlDbType.VarString).Value = txtBairro.Text;
                comando.Parameters.Add("@Cidade", MySqlDbType.VarString).Value = txtCidade.Text;
                comando.Parameters.Add("@UF", MySqlDbType.VarString).Value = txtUf.Text;
                comando.Parameters.Add("@Telefone", MySqlDbType.VarString).Value = mskTelefone.Text;

                try
                {
                    con.Open();
                    comando.ExecuteNonQuery();
                    LimparCampos();
                    MessageBox.Show("Cadastro realizado com sucesso!");

                } catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar no banco de dados: " + ex.Message);
                    con.Close();
                } finally
                {
                    con.Close();
                }
            }
            else 
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de salvar.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtEndereco.Text = "";
            mskCep.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtUf.Text = "";
            mskTelefone.Text = "";
        }

        private bool CamposEstaoPreenchidos()
        {
            string cepSemMascara = new string(mskCep.Text.Where(char.IsDigit).ToArray());
            string telefoneSemMascara = new string(mskTelefone.Text.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                string.IsNullOrWhiteSpace(txtEndereco.Text) ||
                string.IsNullOrWhiteSpace(cepSemMascara) ||
                string.IsNullOrWhiteSpace(txtBairro.Text) ||
                string.IsNullOrWhiteSpace(txtCidade.Text) ||
                string.IsNullOrWhiteSpace(txtUf.Text) ||
                string.IsNullOrWhiteSpace(telefoneSemMascara))
            {
                return false; // Retorna false se algum campo estiver vazio
            }
            return true; // Retorna true se todos os campos estiverem preenchidos
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void tsbPesquisar_Click(object sender, EventArgs e)
        {
            strSql = "SELECT * FROM Funcionarios WHERE Id=@Id";
            var comando = new MySqlCommand(strSql, con);
            
            comando.Parameters.Add("@Id", MySqlDbType.Int32).Value = tstIdBuscar.Text;


            try
            {

                if (tstIdBuscar.Text == string.Empty)
                {
                    throw new Exception("Digite um Id");
                }

                con.Open();

                MySqlDataReader dr = comando.ExecuteReader();

                if (dr.HasRows == false)
                {
                    throw new Exception("Id não cadastrado");
                }

                dr.Read();

                txtId.Text = Convert.ToString(dr["id"]);
                txtNome.Text = Convert.ToString(dr["Nome"]);
                txtEndereco.Text = Convert.ToString(dr["Endereco"]);
                mskCep.Text = Convert.ToString(dr["CEP"]);
                txtBairro.Text = Convert.ToString(dr["Bairro"]);
                txtCidade.Text = Convert.ToString(dr["Cidade"]);
                txtUf.Text = Convert.ToString(dr["UF"]);
                mskTelefone.Text = Convert.ToString(dr["Telefone"]);
            } 
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
                con.Close();
            } finally
            {
                con.Close();
            }

        }
    }
}
