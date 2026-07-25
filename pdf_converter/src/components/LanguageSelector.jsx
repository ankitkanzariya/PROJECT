import { Languages } from 'lucide-react'

function LanguageSelector({ languages, selectedLanguage, onLanguageChange }) {
  return (
    <div className="flex items-center gap-2">
      <Languages className="w-5 h-5 text-purple-400" />
      <select
        value={selectedLanguage}
        onChange={(e) => onLanguageChange(e.target.value)}
        className="bg-white/10 border border-white/20 text-white rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-purple-500 cursor-pointer"
      >
        {languages.map((lang) => (
          <option key={lang.code} value={lang.code} className="text-gray-900">
            {lang.name}
          </option>
        ))}
      </select>
    </div>
  )
}

export default LanguageSelector
