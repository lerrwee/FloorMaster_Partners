using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Data.Entity;
using System.Windows;

namespace FloorMaster_Partners
{
    public partial class PartnerSalesHistoryPage : Page
    {
        public List<Sales> SalesHistory { get; set; }
        private Partner SelectedPartner { get; set; }

        public PartnerSalesHistoryPage(Partner selectedPartner)
        {
            InitializeComponent();

            SelectedPartner = selectedPartner;

            HeaderTextBlock.Text = $"История реализации продукции партнером: {SelectedPartner.partner_name}";

            using (var entity = new FloorsMasterDBEntities())
            {
                SalesHistory = entity.Sales
                    .Include(s => s.Product)
                    .Where(s => s.partner_id == SelectedPartner.id)
                    .ToList();
            }

            DataContext = this;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}