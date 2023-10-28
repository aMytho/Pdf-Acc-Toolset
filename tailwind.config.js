// Copyright (C) Jonathan Shull - See license file at github.com/amytho/pdf-acc-toolset
/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./Pages/**/*.{razor,html}",
        "./Shared/**/*.{razor,html}",
    ],
  theme: {
    extend: {},
  },
  plugins: [
    require("@tailwindcss/typography")
  ],
}

