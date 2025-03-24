using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace FloorMaster_Partners
{
    public partial class AddEditPartnerPage : Page
    {
        private FloorsMasterDBEntities entity;
        private Partner currentPartner;

        public AddEditPartnerPage()
        {
            InitializeComponent();
            entity = new FloorsMasterDBEntities();

            PartnerTypeComboBox.ItemsSource = entity.PartnerType.ToList();
        }

        public AddEditPartnerPage(Partner partner) : this()
        {
            // Извлекаем партнера без отслеживания контекстом
            currentPartner = entity.Partner.AsNoTracking().FirstOrDefault(p => p.id == partner.id);
            LoadPartnerData(currentPartner);
        }

        private void LoadPartnerData(Partner partner)
        {
            NamePartnerTextBox.Text = partner.partner_name;
            PartnerTypeComboBox.SelectedValue = partner.partner_type_id;
            RatingTextBox.Text = partner.rating.ToString();
            AddressTextBox.Text = partner.legal_address;
            FullNameDirectorTextBox.Text = partner.director;
            INNTextBox.Text = partner.inn;
            CompanyPhoneNumberTextBox.Text = partner.phone;
            CompanyEmailTextBox.Text = partner.email;
        }       

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (NamePartnerTextBox.Text == "" || PartnerTypeComboBox.SelectedValue == null || RatingTextBox.Text == "" ||
                AddressTextBox.Text == "" || FullNameDirectorTextBox.Text == "" || INNTextBox.Text == "" ||
                CompanyPhoneNumberTextBox.Text == "" || CompanyEmailTextBox.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!int.TryParse(RatingTextBox.Text, out int rating) || rating < 0)
            {
                MessageBox.Show("Рейтинг партнера должен быть целым неотрицательным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (currentPartner == null)
                {
                    var newPartner = new Partner
                    {
                        partner_name = NamePartnerTextBox.Text,
                        partner_type_id = (int)PartnerTypeComboBox.SelectedValue,
                        rating = rating,
                        legal_address = AddressTextBox.Text,
                        director = FullNameDirectorTextBox.Text,
                        inn = INNTextBox.Text,
                        phone = CompanyPhoneNumberTextBox.Text,
                        email = CompanyEmailTextBox.Text
                    };

                    entity.Partner.Add(newPartner);
                }
                else 
                {
                    // Привязываем объект к текущему контексту
                    var attachedPartner = entity.Partner.Attach(currentPartner);

                    attachedPartner.partner_type_id = Convert.ToInt32(PartnerTypeComboBox.SelectedValue);
                    attachedPartner.partner_name = NamePartnerTextBox.Text;
                    attachedPartner.rating = rating;
                    attachedPartner.legal_address = AddressTextBox.Text;
                    attachedPartner.director = FullNameDirectorTextBox.Text;
                    attachedPartner.inn = INNTextBox.Text;
                    attachedPartner.phone = CompanyPhoneNumberTextBox.Text;
                    attachedPartner.email = CompanyEmailTextBox.Text;

                    // Помечаем объект как измененный
                    entity.Entry(attachedPartner).State = EntityState.Modified;
                }
                entity.SaveChanges();

                MessageBox.Show("Данные сохранены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ListPartnersPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при сохранении данных: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListPartnersPage());
        }
    }
}