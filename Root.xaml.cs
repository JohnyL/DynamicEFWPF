using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DynamicEFWPF
{

	public partial class Root : Window
	{

		TestContext context = null;

		public Root()
		{
			InitializeComponent();
		}

		private void OnWindowLoaded(object sender, RoutedEventArgs e)
		{
			// Создаём контекст
			context = new TestContext(@"Server=(localdb)\MSSQLLocalDB;Database=Test;Trusted_Connection=Yes;");

			// Загрузка справочников
			var dic = new Dictionary<string, string>
			{
				["Стратегия"] = "Strategy",
				["Оператор"] = "Operator",
				["Статус"] = "OrderState"
			};

			foreach (var item in dic)
			{
				cmbDictionaries.Items.Add(new ComboBoxItem { Content = item.Key, Tag = item.Value });
			}
		}

		private async void OnDictionaryChanged(object sender, SelectionChangedEventArgs e)
		{
			var set = GetDbSet();
			set.Load();
			cmbDictionaryEntries.ItemsSource = null;
			cmbDictionaryEntries.ItemsSource = await set.ToListAsync();
		}


		private async void OnChangeEntry(object sender, RoutedEventArgs e)
		{
			var set = GetDbSet();
			dynamic entry = cmbDictionaryEntries.SelectedItem;
			entry.Name = txtEntryName.Text;
			int count = await context.SaveChangesAsync();
			MessageBox.Show($"Сохранено записей: {count}");
		}

		private async void OnAddEntry(object sender, RoutedEventArgs e)
		{
			var set = GetDbSet();
			// Используем dynamic, чтобы проще было добраться до свойства Name
			dynamic entry = set.Create();
			entry.Name = txtEntryName.Text;
			set.Add(entry);
			int count = await context.SaveChangesAsync();
			MessageBox.Show($"Сохранено записей: {count}");
		}


		private DbSet GetDbSet()
		{
			// cmbDictionaries в качестве ItemContainer использует ComboBoxItem.
			// Ранее был показан код добавления значений словаря. В свойство Tag было записано название класса сущности.
			// Здесь SelectedItem возвращает нам Object, а в конечном итоге - ComboBoxItem, у которого мы должны взять Tag.
			// По идее, можно сделать кастинг SelectedItem'a в ComboBoxItem, взять свойство Tag и сконвертировать в String...
			// Но используя dynamic, всё становится намного проще. В итоге setName имеет тип String. В этом можно убедиться,
			// если в Immediate Window выполнить запрос: ?setName.GetType().FullName
			// Ответом будет "System.String".
			dynamic setName = ((dynamic)cmbDictionaries.SelectedItem).Tag;
			// После получения названия целевой сущности, нужно получить её тип.
			// Нужно в строке указать полный путь к классу. Здесь для удобства используется фишка C#7.0 - Interpolation String.
			var type = Type.GetType($"DynamicEFWPF.{setName}");
			// Далее получаем DbSet, но не типизированный (Object).
			var set = context.Set(type);
			return set;
			// Или всё в одну строку:
			//return context.Set(Type.GetType($"DynamicEFWPF.{((dynamic)cmbDictionaries.SelectedItem).Tag}"));
		}

	}
}