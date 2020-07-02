using LS_Report.Models;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LS_Report.Views.VisitesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Questionnaire_View : ContentPage
    {
        private List<tmp_questionnaire> tmp_question { get; set; }

        public Questionnaire_View(List<tmp_questionnaire> tmp_Questionnaire)
        {
            InitializeComponent();
            tmp_question = tmp_Questionnaire;
            int rownumber = 0;
            foreach (var questionnaire in tmp_Questionnaire)
            {
                var questionlabel = new Label
                {
                    Text = questionnaire.question,
                    TextColor = Color.Black,
                    FontSize = 17,
                    FontFamily = "VarelaRound-Regular.ttf#Open Sans",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                };

                var combobox = new SfComboBox
                {
                    DataSource = questionnaire.answers,
                    Text = questionnaire.answer,
                    TextColor = Color.Black,
                    TextSize = 17,
                    FontFamily = "VarelaRound-Regular.ttf#Open Sans",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    ShowBorder = false,
                    ClassId = questionnaire.id_question
                };
                Questionnaire_Grid.RowDefinitions.Add(new RowDefinition
                {
                    Height = 70
                });
                questionlabel.SetValue(Grid.RowProperty, rownumber);
                questionlabel.SetValue(Grid.ColumnProperty, 0);
                combobox.SetValue(Grid.RowProperty, rownumber);
                combobox.SetValue(Grid.ColumnProperty, 1);
                Questionnaire_Grid.Children.Add(questionlabel);
                Questionnaire_Grid.Children.Add(combobox);
                rownumber += 1;
            }
            Add_Button.SetValue(Grid.RowProperty, rownumber + 1);
        }

        private async void click(object sender, EventArgs e)
        {
            foreach (var row in Questionnaire_Grid.Children)
            {
                if (row is SfComboBox)
                {
                    var cmbx = (SfComboBox)row;
                    if (cmbx.SelectedItem != null)
                    {
                        tmp_question.Single(i => i.id_question == cmbx.ClassId).answer = cmbx.SelectedItem.ToString();
                    }
                }
            }
            MessagingCenter.Send(this, "QuestionnaireUpdated", tmp_question);
            await Navigation.PopModalAsync();
        }
    }
}