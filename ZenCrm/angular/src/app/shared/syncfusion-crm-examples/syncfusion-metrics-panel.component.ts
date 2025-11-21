import { Component } from '@angular/core';

@Component({
  selector: 'app-syncfusion-metrics-panel',
  standalone: true,
  template: `
    <div class="mt-4">
      <div class="flex justify-center">
        <div class="w-full max-w-7xl">
          <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
            <!-- Total Revenue Card -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-blue-500 to-blue-600 text-white p-4">
                <div class="text-sm opacity-90">Total Revenue</div>
                <div class="text-2xl font-bold">{{ metricsData.totalRevenue.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-blue-400 rounded">{{ metricsData.totalRevenue.change }}</span>
                </div>
              </div>
            </div>

            <!-- New Deals Card -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-green-500 to-green-600 text-white p-4">
                <div class="text-sm opacity-90">New Deals</div>
                <div class="text-2xl font-bold">{{ metricsData.newDeals.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-green-400 rounded">{{ metricsData.newDeals.change }}</span>
                </div>
              </div>
            </div>

            <!-- Pipeline Value Card -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-orange-500 to-orange-600 text-white p-4">
                <div class="text-sm opacity-90">Pipeline Value</div>
                <div class="text-2xl font-bold">{{ metricsData.pipelineValue.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-orange-400 rounded">{{ metricsData.pipelineValue.change }}</span>
                </div>
              </div>
            </div>

            <!-- Win Rate Card -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-purple-500 to-purple-600 text-white p-4">
                <div class="text-sm opacity-90">Win Rate</div>
                <div class="text-2xl font-bold">{{ metricsData.winRate.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-purple-400 rounded">{{ metricsData.winRate.change }}</span>
                </div>
              </div>
            </div>
          </div>

          <!-- Additional Metrics Row -->
          <div class="grid grid-cols-1 md:grid-cols-3 gap-6 mt-6">
            <!-- Sales Growth -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-teal-500 to-teal-600 text-white p-4">
                <div class="text-sm opacity-90">Sales Growth</div>
                <div class="text-2xl font-bold">{{ metricsData.salesGrowth.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-teal-400 rounded">{{ metricsData.salesGrowth.change }}</span>
                </div>
              </div>
            </div>

            <!-- Customer Satisfaction -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-pink-500 to-pink-600 text-white p-4">
                <div class="text-sm opacity-90">Customer Satisfaction</div>
                <div class="text-2xl font-bold">{{ metricsData.customerSatisfaction.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-pink-400 rounded">{{ metricsData.customerSatisfaction.change }}</span>
                </div>
              </div>
            </div>

            <!-- Active Leads -->
            <div class="bg-white rounded-lg shadow-lg overflow-hidden">
              <div class="bg-gradient-to-r from-indigo-500 to-indigo-600 text-white p-4">
                <div class="text-sm opacity-90">Active Leads</div>
                <div class="text-2xl font-bold">{{ metricsData.activeLeads.value }}</div>
                <div class="text-xs mt-1">
                  <span class="inline-block px-2 py-1 bg-indigo-400 rounded">{{ metricsData.activeLeads.change }}</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    /* Custom CSS to simulate Tailwind classes since we don't have Tailwind */
    .mt-4 { margin-top: 1rem; }
    .flex { display: flex; }
    .justify-center { justify-content: center; }
    .w-full { width: 100%; }
    .max-w-7xl { max-width: 80rem; }
    .grid { display: grid; }
    .grid-cols-1 { grid-template-columns: repeat(1, minmax(0, 1fr)); }
    .md\\:grid-cols-2 {
      @media (min-width: 768px) {
        grid-template-columns: repeat(2, minmax(0, 1fr));
      }
    }
    .lg\\:grid-cols-4 {
      @media (min-width: 1024px) {
        grid-template-columns: repeat(4, minmax(0, 1fr));
      }
    }
    .md\\:grid-cols-3 {
      @media (min-width: 768px) {
        grid-template-columns: repeat(3, minmax(0, 1fr));
      }
    }
    .gap-6 { gap: 1.5rem; }
    .bg-white { background-color: white; }
    .rounded-lg { border-radius: 0.5rem; }
    .shadow-lg {
      box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1), 0 4px 6px -2px rgba(0, 0, 0, 0.05);
    }
    .overflow-hidden { overflow: hidden; }
    .bg-gradient-to-r { background-image: linear-gradient(to right, var(--tw-gradient-stops)); }
    .from-blue-500 { --tw-gradient-from: #3b82f6; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(59 130 246 / 0)); }
    .to-blue-600 { --tw-gradient-to: #2563eb; }
    .from-green-500 { --tw-gradient-from: #10b981; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(16 185 129 / 0)); }
    .to-green-600 { --tw-gradient-to: #059669; }
    .from-orange-500 { --tw-gradient-from: #f97316; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(249 115 22 / 0)); }
    .to-orange-600 { --tw-gradient-to: #ea580c; }
    .from-purple-500 { --tw-gradient-from: #a855f7; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(168 85 247 / 0)); }
    .to-purple-600 { --tw-gradient-to: #9333ea; }
    .from-teal-500 { --tw-gradient-from: #14b8a6; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(20 184 166 / 0)); }
    .to-teal-600 { --tw-gradient-to: #0d9488; }
    .from-pink-500 { --tw-gradient-from: #ec4899; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(236 72 153 / 0)); }
    .to-pink-600 { --tw-gradient-to: #db2777; }
    .from-indigo-500 { --tw-gradient-from: #6366f1; --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to, rgb(99 102 241 / 0)); }
    .to-indigo-600 { --tw-gradient-to: #4f46e5; }
    .text-white { color: white; }
    .p-4 { padding: 1rem; }
    .text-sm { font-size: 0.875rem; line-height: 1.25rem; }
    .opacity-90 { opacity: 0.9; }
    .text-2xl { font-size: 1.5rem; line-height: 2rem; }
    .font-bold { font-weight: 700; }
    .text-xs { font-size: 0.75rem; line-height: 1rem; }
    .mt-1 { margin-top: 0.25rem; }
    .inline-block { display: inline-block; }
    .px-2 { padding-left: 0.5rem; padding-right: 0.5rem; }
    .py-1 { padding-top: 0.25rem; padding-bottom: 0.25rem; }
    .bg-blue-400 { background-color: #60a5fa; }
    .bg-green-400 { background-color: #4ade80; }
    .bg-orange-400 { background-color: #fb923c; }
    .bg-purple-400 { background-color: #c084fc; }
    .bg-teal-400 { background-color: #2dd4bf; }
    .bg-pink-400 { background-color: #f472b6; }
    .bg-indigo-400 { background-color: #818cf8; }
    .rounded { border-radius: 0.25rem; }

    @media (max-width: 768px) {
      .gap-6 {
        gap: 1rem;
      }
      .text-2xl {
        font-size: 1.25rem;
        line-height: 1.75rem;
      }
    }
  `]
})
export class SyncfusionMetricsPanelComponent {
  metricsData = {
    totalRevenue: {
      value: '$2.4M',
      change: '+12.5% from last month'
    },
    newDeals: {
      value: '48',
      change: '+8 deals this week'
    },
    pipelineValue: {
      value: '$8.7M',
      change: '+23% from last quarter'
    },
    winRate: {
      value: '72%',
      change: '+5% improvement'
    },
    salesGrowth: {
      value: '28%',
      change: 'Year over year'
    },
    customerSatisfaction: {
      value: '4.8/5.0',
      change: '+0.3 this quarter'
    },
    activeLeads: {
      value: '156',
      change: '+24 new leads'
    }
  };
}