using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace FloorMaster_Partners
{
    public partial class CalculateMaterialPage : Page
    {
        private FloorsMasterDBEntities entity;

        public CalculateMaterialPage()
        {
            InitializeComponent();
            entity = new FloorsMasterDBEntities();

            var productTypes = entity.ProductType.ToList();
            ProductTypeComboBox.ItemsSource = productTypes;

            var materialTypes = entity.MaterialType.ToList();
            MaterialTypeComboBox.ItemsSource = materialTypes;
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedProductType = ProductTypeComboBox.SelectedItem as ProductType;
                var selectedMaterialType = MaterialTypeComboBox.SelectedItem as MaterialType;

                if (selectedProductType == null || selectedMaterialType == null)
                {
                    MessageBox.Show("Пожалуйста, выберите тип продукции и тип материала.","Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!int.TryParse(CountTextBox.Text, out int count) || count <= 0)
                {
                    MessageBox.Show("Количество продукции должно быть целым положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!float.TryParse(WidthTextBox.Text, out float width) || width <= 0)
                {
                    MessageBox.Show("Ширина продукции должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!float.TryParse(LengthTextBox.Text, out float length) || length <= 0)
                {
                    MessageBox.Show("Длина продукции должна быть положительным числом.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                count = int.Parse(CountTextBox.Text);
                width = float.Parse(WidthTextBox.Text);
                length = float.Parse(LengthTextBox.Text);

                double productTypeCoefficient = selectedProductType.type_coefficient;
                double defectPercentage = selectedMaterialType.defect_percentage;

                int requiredMaterial = GetQuantityForProduct(productTypeCoefficient, defectPercentage, count, width, length);

                ResultTextBlock.Text = $"Необходимое количество материала: {requiredMaterial}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при расчете: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int GetQuantityForProduct(double productTypeCoefficient, double defectPercentage, int count, float width, float length)
        {
            if (count <= 0 || width <= 0 || length <= 0)
            {
                return -1;
            }

            double area = width * length;
            double materialPerUnit = area * productTypeCoefficient;
            double totalMaterial = materialPerUnit * count;

            double totalMaterialWithDefect = totalMaterial / (1 - defectPercentage);

            int requiredMaterial = (int)Math.Ceiling(totalMaterialWithDefect);

            return requiredMaterial;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ListPartnersPage());
        }
    }
}