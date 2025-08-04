using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Abstractions;

namespace BH_PhotoManager.Common.Navigation
{
    public class NavigationViewPageProvider : INavigationViewPageProvider
    {
        private readonly Dictionary<string, Type> _pages = new();

        public NavigationViewPageProvider()
        {
            RegisterPagesFromAssembly(Assembly.GetExecutingAssembly());
        }

        public void Register(string pageTag, Type pageType)
        {
            if (!typeof(Page).IsAssignableFrom(pageType))
                throw new ArgumentException($"{pageType.FullName} does not inherit from Page");

            if (!_pages.ContainsKey(pageTag))
                _pages.Add(pageTag, pageType);
        }

        public Type? GetPageType(string pageTag)
        {
            _pages.TryGetValue(pageTag, out var pageType);
            return pageType;
        }

        public IReadOnlyDictionary<string, Type> GetRegisteredPages() => _pages;

        private void RegisterPagesFromAssembly(Assembly assembly)
        {
            var pageTypes = assembly.GetTypes()
                .Where(t => typeof(Page).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract);

            foreach (var pageType in pageTypes)
            {
                // 자동 등록: 페이지 클래스 이름을 tag로 사용
                var pageTag = pageType.Name.Replace("Page", "").ToLower();
                Register(pageTag, pageType);
            }
        }

        public object? GetPage(Type pageType)
        {
            throw new NotImplementedException();
        }
    }
}
