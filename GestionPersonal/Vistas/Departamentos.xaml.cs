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
        DataRow empleadoActual;
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
            dtgDep.ItemsSource = eliminarColumnasDepartamento(dtDepartamentos).DefaultView;
        }


        private DataTable eliminarColumnasDepartamento(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("IdJefeDep");
            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("NombreE");
            dtShow.Columns.Remove("Apellido");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("Borrado");

            return dtShow;
        }

        private DataTable eliminarColumnasEmpleado(DataTable dt)
        {
            DataTable dtShow = dt.Copy();

            dtShow.Columns.Remove("IdEmpleado");
            dtShow.Columns.Remove("Contrasenia");
            dtShow.Columns.Remove("NumSS");
            dtShow.Columns.Remove("EstadoE");
            dtShow.Columns.Remove("DNI");
            dtShow.Columns.Remove("Rol");
            dtShow.Columns.Remove("Estado");
            dtShow.Columns.Remove("IdDepartamento");
            dtShow.Columns.Remove("FechaUltModif");
            dtShow.Columns.Remove("Borrado");
            dtShow.Columns.Remove("IdModif");
            dtShow.Columns.Remove("NombreD");

            return dtShow;
        }

        private void btnCrear_Click(object sender, RoutedEventArgs e)
        {
            if (controladorDepartamento.crearDepartamento(txbNombreD.Text, txbDescripcionD.Text) == true)
            {
                MessageBox.Show("Departamento creado con éxito");
                cargarDTG(string.Empty);
                vaciarCampos();
            } 
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (departamentoActual["IdDepartamento"].ToString() != string.Empty)
            {
                if (hayCambios)
                {
                    controladorDepartamento.modificarDepartamento(departamentoActual);
                    MessageBox.Show("Datos guardados correctamente");
                    cargarDTG(string.Empty);
                    departamentoActual = dtDepartamentos.Copy().Rows[contDepartamento];
                    hayCambios = false;
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
                        vaciarCampos();
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
                contDepartamento = dtgDep.SelectedIndex;
                departamentoActual = dtDepartamentos.Copy().Rows[contDepartamento];
                cargarDepartamento();

                cargarEmpleadosDepartamento();
            }
        }

        private void cargarDepartamento()
        {
            hayCambios = false;
            dblClic = true;

            txbNombreD.Text = departamentoActual["NombreD"].ToString();
            txbDescripcionD.Text = departamentoActual["DescripcionD"].ToString();
            txbJefe.Text = departamentoActual["NombreE"].ToString() + " " + departamentoActual["Apellido"].ToString();

            dblClic = false;
        }

        private void cargarEmpleadosDepartamento()
        {
            dtEmpleadosDep = controladorDepartamento.listaEmpleadosDepartamento(departamentoActual["IdDepartamento"].ToString());
            empleadoActual = dtEmpleadosDep.NewRow();
            dtgEmpleadosDep.ItemsSource = null;
            dtgEmpleadosDep.ItemsSource = eliminarColumnasEmpleado(dtEmpleadosDep).DefaultView;
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

        private void btnAddE_Click(object sender, RoutedEventArgs e)
        {
            controladorDepartamento.prepararFiltroEmpleado();
        }

        private void Window_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (this.IsEnabled)
            {
                controladorDepartamento.aniadirEmpleado(departamentoActual["IdDepartamento"].ToString());
                cargarEmpleadosDepartamento();
            }
        }

        private void btnJefe_Click(object sender, RoutedEventArgs e)
        {
            if (dtgEmpleadosDep.SelectedItem != null)
            {
                DialogResult dr = MessageBox.Show("¿Desea nombrar a " + dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["NombreE"].ToString() +
                    " " + dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["Apellido"].ToString() + " como jefe del departamento " +
                    departamentoActual["NombreD"].ToString() + "?", "Confirmación", MessageBoxButtons.YesNo);
                if (dr == System.Windows.Forms.DialogResult.Yes)
                {
                    if(controladorDepartamento.asignarJefe(dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["IdEmpleado"].ToString(),
                        departamentoActual["IdDepartamento"].ToString()))
                    {
                        dblClic = true;
                        cargarDTG(string.Empty);
                        departamentoActual["NombreE"] = dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["NombreE"].ToString();
                        departamentoActual["Apellido"] = dtEmpleadosDep.Rows[dtgEmpleadosDep.SelectedIndex]["Apellido"].ToString();
                        cargarDepartamento();
                    }
                        
                }
            }
            else
                MessageBox.Show("Debe seleccionar un empleado.");
        }

        private void btnVacio_Click(object sender, RoutedEventArgs e)
        {
            cargarDTG(string.Empty);
            vaciarCampos();
        }

        private void vaciarCampos()
        {
            dblClic = true;
            departamentoActual = dtDepartamentos.NewRow();
            
            txbBucarDep.Text = "Buscar departamento...";
            txbBucarDep.GotFocus+= txbBucarDep_GotFocus;

            txbDescripcionD.Text= string.Empty;
            txbNombreD.Text= string.Empty;
            txbJefe.Text= string.Empty;

            dtgDep.SelectedItem = null;
            dtgEmpleadosDep.SelectedItem= null;
            dtgEmpleadosDep.ItemsSource = null;

            dblClic = false;
            hayCambios = false;
        }
    }
}
