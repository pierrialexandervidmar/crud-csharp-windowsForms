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
            } else
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de salvar.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            acoesCancelarSalvar();
            LimparCampos();
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            tsbNovo.Enabled = true;
            tsbSalvar.Enabled = false;
            tsbEditar.Enabled = false;
            tsbCancelar.Enabled = false;
            tsbExcluir.Enabled = false;
            tsbPesquisar.Enabled = true;
            txtId.Enabled = false;
            txtNome.Enabled = false;
            txtEndereco.Enabled = false;
            mskCep.Enabled = false;
            txtBairro.Enabled = false;
            txtCidade.Enabled = false;
            txtUf.Enabled = false;
            mskTelefone.Enabled = false;
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
            } catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
                con.Close();
            } finally
            {
                con.Close();
            }

            tsbNovo.Enabled = false;
            tsbSalvar.Enabled = false;
            tsbEditar.Enabled = true;
            tsbCancelar.Enabled = true;
            tsbExcluir.Enabled = false;
            tsbPesquisar.Enabled = false;
            txtId.Enabled = false;
            txtNome.Enabled = true;
            txtEndereco.Enabled = true;
            mskCep.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtUf.Enabled = true;
            mskTelefone.Enabled = true;

        }

        private void tsbEditar_Click(object sender, EventArgs e)
        {
            if (CamposEstaoPreenchidos())
            {
                strSql = "UPDATE Funcionarios SET Nome = @Nome, Endereco = @Endereco, CEP = @CEP, Bairro = @Bairro, Cidade = @Cidade, UF = @UF, Telefone = @Telefone WHERE id = @IdBuscar";
                var comando = new MySqlCommand(strSql, con);

                comando.Parameters.Add("@IdBuscar", MySqlDbType.Int32).Value = tstIdBuscar.Text;

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
                    MessageBox.Show("Atualização realizada com sucesso!");

                } catch (Exception ex)
                {
                    MessageBox.Show("Erro ao conectar no banco de dados: " + ex.Message);
                    con.Close();
                } finally
                {
                    con.Close();
                }
            } else
            {
                MessageBox.Show("Por favor, preencha todos os campos antes de salvar.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void tsbExcluir_Click(object sender, EventArgs e)
        {
            if (CamposEstaoPreenchidos() || txtId.Text != "")
            {
                if (MessageBox.Show("Deseja realmente excluir este funcionário?", "Atenção",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    MessageBox.Show("Operação Cancelada");

                } else
                {
                    strSql = "DELETE FROM Funcionarios WHERE Id=@Id";
                    var comando = new MySqlCommand(strSql, con);

                    comando.Parameters.Add("@Id", MySqlDbType.Int32).Value = tstIdBuscar.Text;

                    try
                    {
                        con.Open();
                        comando.ExecuteNonQuery();
                        LimparCampos();
                        MessageBox.Show("Exclusão realizada com sucesso!");

                    } catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao conectar no banco de dados: " + ex.Message);
                        con.Close();
                    } finally
                    {
                        con.Close();
                    }
                }
            } else
            {
                MessageBox.Show("Por favor, consulte antes para que preencha todos os campos antes de excluir.", "Campos Vazios", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void LimparCampos()
        {
            txtId.Text = "";
            txtNome.Text = "";
            txtEndereco.Text = "";
            mskCep.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtUf.Text = "";
            mskTelefone.Text = "";
            tstIdBuscar.Text = "";
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

        private void tsbNovo_Click(object sender, EventArgs e)
        {
            tsbNovo.Enabled = false;
            tsbSalvar.Enabled = true;
            tsbEditar.Enabled = false;
            tsbCancelar.Enabled = true;
            tsbExcluir.Enabled = false;
            tsbPesquisar.Enabled = false;
            txtId.Enabled = false;
            txtNome.Enabled = true;
            txtEndereco.Enabled = true;
            mskCep.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtUf.Enabled = true;
            mskTelefone.Enabled = true;
            tstIdBuscar.Enabled = false;
        }

        private void tsbCancelar_Click(object sender, EventArgs e)
        {
            acoesCancelarSalvar();
        }

        private void acoesCancelarSalvar()
        {
            LimparCampos();

            tsbNovo.Enabled = true;
            tsbSalvar.Enabled = false;
            tsbEditar.Enabled = false;
            tsbCancelar.Enabled = false;
            tsbExcluir.Enabled = false;
            tsbPesquisar.Enabled = true;
            txtId.Enabled = false;
            txtNome.Enabled = false;
            txtEndereco.Enabled = false;
            mskCep.Enabled = false;
            txtBairro.Enabled = false;
            txtCidade.Enabled = false;
            txtUf.Enabled = false;
            mskTelefone.Enabled = false;
            tstIdBuscar.Enabled = true;
        }
    }
}
