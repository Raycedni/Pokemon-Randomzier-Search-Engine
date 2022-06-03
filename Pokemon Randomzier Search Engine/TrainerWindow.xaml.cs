using Pokemon_Randomzier_Search_Engine.backend;
using Pokemon_Typings.backend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pokemon_Randomzier_Search_Engine
{
    /// <summary>
    /// Interaction logic for TrainerWindow.xaml
    /// </summary>
    public partial class TrainerWindow : Window
    {
        MainWindow mainWindow;

        public TrainerWindow(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            initializeTrainerNamesCombobox(this.mainWindow.pokemonDatabase);
        }

        private void initializeTrainerNamesCombobox(RandomizerDatabase database)
        {
            foreach(string trainer in database.getTrainerNames())
            {
                comboBoxTrainerRandomizedName.Items.Add(trainer);
            }
            comboBoxTrainerRandomizedName.SelectedIndex = 0;
            fillTrainerEncounterList(comboBoxTrainerRandomizedName.Text);
            fillPokemonList(comboBoxTrainerRandomizedName.Text, int.Parse(comboBoxTrainerEncounter.Text));
        }

        private void fillTrainerEncounterList(string trainerName)
        {
            comboBoxTrainerEncounter.Items.Clear();
            for(int i = 0; i < mainWindow.pokemonDatabase.getTrainer(trainerName).Count; i++)
            {
                comboBoxTrainerEncounter.Items.Add(i+1);
            }
            comboBoxTrainerEncounter.SelectedIndex = 0;
        }

        private void fillPokemonList(string trainerName, int encounter)
        {
            listboxPokemonList.Items.Clear();

            Trainer trainer = mainWindow.pokemonDatabase.getTrainer(trainerName)[encounter - 1];
            foreach(Pokemon pokemon in trainer.pokemonList)
            {
                listboxPokemonList.Items.Add(pokemon.name);
            }
        }

        private void listboxPokemonList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (listboxPokemonList.SelectedIndex > -1)
                {
                    mainWindow.comboBoxSearch.Text = listboxPokemonList.SelectedItem.ToString();
                    mainWindow.searchInDB(listboxPokemonList.SelectedItem.ToString());
                }
            }catch(Exception easda)
            {
                MessageBox.Show(easda.Message);
            }
        }

        private void comboBoxTrainerRandomizedName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                fillTrainerEncounterList(comboBoxTrainerRandomizedName.Text);
                fillPokemonList(comboBoxTrainerRandomizedName.Text, int.Parse(comboBoxTrainerEncounter.Text));
            }
        }

        private void comboBoxTrainerEncounter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxTrainerEncounter.Text != "")
                fillPokemonList(comboBoxTrainerRandomizedName.Text, int.Parse(comboBoxTrainerEncounter.Text));
        }
    }
}
