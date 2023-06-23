using GestionPersonal.Utiles;
using GestionPersonal.Vistas;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MessageBox = System.Windows.Forms.MessageBox;

namespace GestionPersonal
{

    /// <summary>
    /// Lógica de interacción para Departamentos.xaml
    /// </summary>
    public partial class Departamentos : Window
    {
        private readonly DepartamentoControl controladorDepartamento;
        DataTable dtDepartamentos;
        DataTable dtEmpleadosDep;
        DataRow departamentoActual;
        bool dblClic;
        bool hayCambios;
        int contDepartamento;

        public Departamentos(DepartamentoControl controladorDepartamento)
        {
            this.controladorDepartamento = controladorDepartamento;

            InitializeComponent();

            cargarDTG(string.Empty);

            departamentoActual = dtDepartamentos.NewRow();
        }


        private void cargarDTG(string filtro)
        {
            if (filtro == string.Empty)
            {
                dtDepartamentos = controladorDepartamento.listaDepartamentos();
            }
            else
            {
                dtDepartamentos = controladorDepartamento.listaDepartamentos(filtro);
            }

            dtgDep.ItemsSource = null;
            dtgDep.ItemsSource = eliminarColumnas(dtDepartamentos).DefaultView;
        }


        private DataTable eliminarColumnas(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("IdJefeDep");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("Borrado");

            return dtShow;
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorDepartamento.crearDepartamento(txbNombreD.Text, txbDescripcionD.Text) == true)
            {
                MessageBox.Show("Departamento creado con éxito");
                cargarDTG(string.Empty);
            } 
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (departamentoActual["IdDepartamento"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    controladorDepartamento.modificarDepartamento(departamentoActual);
                    hayCambios = false;
                    MessageBox.Show("Datos guardados correctamente");
                    cargarDTG(string.Empty);
                } 
            }
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            if (dtgDep.SelectedItem != null)
            {
                //if (dtEmpleadosDep.Rows.Count == 0)
                //{
                    DialogResult dr = MessageBox.Show("¿Eliminar el DEPARTAMENTO seleccionado?", "Eliminar Departamento",
                   MessageBoxButtons.YesNo);

                    if (dr == System.Windows.Forms.DialogResult.Yes)
                    {
                        controladorDepartamento.eliminarDepartamento(dtDepartamentos.Rows[dtgDep.SelectedIndex]["IdDepartamento"].ToString());
                        cargarDTG(string.Empty);
                    }
                    else
                        return;
                //}
                //else
                    //MessageBox.Show("El departamento no puede contener ningún empleado a la hora de ser eliminado.");
            }
            else
                System.Windows.MessageBox.Show("Seleccione un departamento de la tabla");
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            controladorDepartamento.volverMenu();
        }

        private void dtgDep_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dtgDep.SelectedItem != null)
            {
                dblClic = true;
                hayCambios = false;

                contDepartamento = dtgDep.SelectedIndex;
                departamentoActual = dtDepartamentos.Copy().Rows[contDepartamento];
                cargarDepartamento();
            }
        }

        private void cargarDepartamento()
        {
            txbNombreD.Text = departamentoActual["NombreD"].ToString();
            txbDescripcionD.Text = departamentoActual["DescripcionD"].ToString();
            //txbJefe.Text = NOMBRE DEL JEFE y añadir botón para visitarlo en menú empleados

            dblClic = false;
        }


        /// <summary>
        /// Guarda los cambios del TextBox en la fila referente al departamento actual. El atributo Name del elemento
        /// cambiado ha de tener sus 3 primeras letras descartables y lo demás ha de coincidir con el nombre del atributo
        /// en el datatable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!dblClic)//Esto es para que al cargar los Txb después del dtg dobleclick, no haga esto.
            {
                if (departamentoActual["IdDepartamento"].ToString() != string.Empty)
                {
                    string columna = ((System.Windows.Controls.TextBox)sender).Name.Substring(3);
                    departamentoActual[columna] = ((System.Windows.Controls.TextBox)sender).Text;
                }
                hayCambios = true;
            }
        }



        private void dtgDep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dtgDep.SelectedItem != null) 
            {
                
            }
        }

        private void txbBucarDep_GotFocus(object sender, RoutedEventArgs e)
        {
            txbBucarDep.Text= string.Empty;
            txbBucarDep.GotFocus -= txbBucarDep_GotFocus;
            txbBucarDep.TextChanged += txbBucarDep_TextChanged;
        }

        private void txbBucarDep_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filtro = $"NombreD like '%{txbBucarDep.Text}%'";
            cargarDTG(filtro);
        }

    }
}
