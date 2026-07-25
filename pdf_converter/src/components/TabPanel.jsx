import { X } from 'lucide-react'

function TabPanel({ tabs, activeTab, onTabChange, onCloseTab }) {
  return (
    <div className="flex border-b border-white/10 bg-white/5">
      {tabs.map((tab) => (
        <div
          key={tab.id}
          className={`flex items-center gap-2 px-4 py-3 cursor-pointer transition-colors border-r border-white/10 ${
            activeTab === tab.id
              ? 'bg-purple-600 text-white'
              : 'text-purple-200 hover:bg-white/10'
          }`}
          onClick={() => onTabChange(tab.id)}
        >
          <span className="font-medium">{tab.label}</span>
          {tab.id !== 'original' && (
            <button
              onClick={(e) => {
                e.stopPropagation()
                onCloseTab(tab.id)
              }}
              className="ml-2 hover:text-white transition-colors"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>
      ))}
    </div>
  )
}

export default TabPanel
