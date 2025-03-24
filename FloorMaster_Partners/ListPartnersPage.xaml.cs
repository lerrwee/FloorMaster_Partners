using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Data.Entity;

namespace FloorMaster_Partners
{
    public partial class ListPartnersPage : Page
    {
        FloorsMasterDBEntities entity;
        public List<Partner> Partners { get; set; }

        public ListPartnersPage()
        {
            InitializeComponent();
            entity = new FloorsMasterDBEntities();
            Partners = entity.Partner.Include(p => p.PartnerType).ToList();
            CalculateDiscountsForPartners();
            PartnersListView.ItemsSource = Partners;
        }

        private void CalculateDiscountsForPartners()
        {
            foreach (var partner in Partners)
            {
                int? totalSales = entity.Sales
                    .Where(s => s.partner_id == partner.id)
                    .Sum(s => (int?)s.quantity); 

                partner.Tag = CalculateDiscount(totalSales ?? 0).ToString() + "%";
            }
        }

        private int CalculateDiscount(int totalSales)
        {
            if (totalSales >= 300000)
                return 15;
            else if (totalSales >= 50000)
                return 10;
            else if (totalSales >= 10000)
                return 5;
            else
                return 0;
        }

        private void EditPartner_DoubleClick(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedPartner = PartnersListView.SelectedItem as Partner;
            if (selectedPartner != null)
            {
                NavigationService.Navigate(new AddEditPartnerPage(selectedPartner));
            }
        }

        private void PartnerSalesHistory_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedPartner = PartnersListView.SelectedItem as Partner;
            if (selectedPartner != null)
            {
                NavigationService.Navigate(new PartnerSalesHistoryPage(selectedPartner));
            }
        }

        private void AddPartnerButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditPartnerPage());
        }

        private void CalculateMaterialButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new CalculateMaterialPage());
        }
    }     
}