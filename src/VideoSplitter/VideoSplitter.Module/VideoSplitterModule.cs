using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using VideoSplitter.Module.Views;

namespace VideoSplitter.Module
{
    public class VideoSplitterModule : IModule
    {
        private readonly IRegionManager _regionManager;


        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(HomeView));
        }

        public VideoSplitterModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}
