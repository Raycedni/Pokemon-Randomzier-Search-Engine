using Microsoft.Win32;
using Pokemon_Typings.backend;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Pokemon_Randomzier_Search_Engine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileHandler fileHandler = new FileHandler();
        public RandomizerDatabase pokemonDatabase = new RandomizerDatabase();
        string fileContent;
        TrainerWindow trainerWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void tbxInputFile_TextChanged(object sender, TextChangedEventArgs e)
        {
            initialzeComponents(tbxInputFile.Text);
        }

        private void ColumnDefinition_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                tbxInputFile.Text = files[0];
            }
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                tbxInputFile.Text = openFileDialog.FileName;

                initialzeComponents(tbxInputFile.Text);
            }
        }

        private void initDB(RandomizerDatabase pokemonDatabase, string logFileContent)
        {
            pokemonDatabase.fillPokemonDatabase(
                    fileHandler.getPokemonBaseStats_Types(logFileContent),
                    fileHandler.getPokemonMoves(logFileContent));
        }

        private void fillComboBoxWithPokemon(ComboBox comboBox, RandomizerDatabase pokemonDatabase)
        {
            comboBox.IsEnabled = true;
            foreach (string pokemon in pokemonDatabase.getRegisteredPokemon())
            {
                comboBox.Items.Add(pokemon);
            }
            comboBox.Text = pokemonDatabase.getPokemonAtIndex(0).name;
            comboBoxSearch.Text = comboBoxSearch.Items[comboBoxSearch.SelectedIndex].ToString();
            searchInDB(comboBoxSearch.Text);
        }

        private void initialzeComponents(string logFile)
        {
            try
            {
                if (File.Exists(logFile))
                {
                    fileContent = fileHandler.getFileContent(logFile);
                    initDB(pokemonDatabase, fileContent);
                    fillComboBoxWithPokemon(comboBoxSearch, pokemonDatabase);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void searchInDB(string searchString)
        {
            Grid grid;
            TextBox textBox;
            try
            {
                List<Pokemon> pokemonFamily = pokemonDatabase.getPokemonFamily(searchString);

                foreach (Object mainGridOject in mainGrid.Children)
                {
                    if (mainGridOject.GetType() == typeof(Grid))
                    {
                        grid = (Grid)mainGridOject;
                        foreach (Object childGridObject in grid.Children)
                        {
                            if (childGridObject.GetType() == typeof(TextBox))
                            {
                                textBox = (TextBox)childGridObject;
                                setTextboxText(textBox, pokemonFamily);
                            }
                        }
                    }

                    if (mainGridOject.GetType() == typeof(TextBox))
                    {
                        textBox = (TextBox)mainGridOject;
                        setTextboxText(textBox, pokemonFamily);
                    }
                }
            }
            catch (PokemonNotFoundException e)
            {
                MessageBox.Show(e.Message, "Error");
            }
        }

        private void setTextboxText(TextBox textBox, List<Pokemon> pokemonFamily)
        {
            int currentEvolution;
            textBox.IsEnabled = true;

            if (textBox.Name.Contains("EvolutionName"))
            {
                currentEvolution = int.Parse(textBox.Name.Substring(textBox.Name.Length - 1, 1)) - 1;
                if (pokemonFamily.Count - 1 >= currentEvolution)
                    textBox.Text = pokemonFamily[currentEvolution].name;
                else
                {
                    textBox.IsEnabled = false;
                    textBox.Text = "";
                }

            }

            if (textBox.Name.Contains("EvolutionType"))
            {
                currentEvolution = int.Parse(textBox.Name.Substring(textBox.Name.Length - 1, 1)) - 1;
                if (pokemonFamily.Count - 1 >= currentEvolution)
                    textBox.Text = pokemonFamily[currentEvolution].type;
                else
                {
                    textBox.IsEnabled = false;
                    textBox.Text = "";
                }
            }

            if (textBox.Name.Contains("EvolutionMoves"))
            {
                currentEvolution = int.Parse(textBox.Name.Substring(textBox.Name.Length - 1, 1)) - 1;
                if (pokemonFamily.Count - 1 >= currentEvolution)
                    textBox.Text = pokemonFamily[currentEvolution].moves;
                else
                {
                    textBox.IsEnabled = false;
                    textBox.Text = "";
                }
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            tbxInputFile.Text = Properties.Settings.Default.ndsLogFile;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.ndsLogFile = tbxInputFile.Text;
            Properties.Settings.Default.Save();

            if(trainerWindow != null)
                trainerWindow.Close();
        }

        private void TbxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                searchInDB(comboBoxSearch.Text);
            }
        }

        private void btnOpenTrainwerWindow_Click(object sender, RoutedEventArgs e)
        {
            pokemonDatabase.fillTrainerDatabase(fileHandler.getTrainers(fileContent));
            trainerWindow = new TrainerWindow(this);

            trainerWindow.Show();
        }
    }
}
