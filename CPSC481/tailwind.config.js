const colors = require("tailwindcss/colors");
module.exports = {
	purge: {
		enabled: true,
		content: ["./**/*.html", "./**/*.razor"],
	},
	darkMode: false,
	theme: {
		fontFamily: {
			sans: ["Poppins", "sans-serif"],
		},
		extend: {
			colors: {
				primary: "#1c5d99",
				"primary-2": "#133f67",
				secondary: "#f3efe0",
				"secondary-2": "#eee8d2",
				"secondary-3": "#e3d9b6",
				neutral: "#8A929C",
				"neutral-2": "#6c747f",
				black: "#353535",
				red: "#ff8966",
				"red-2": "#c82e00",
				green: "#00b865",
				"green-2": "#00df7b",
				blue: "#1c5d99",
				blue2: "#257bcb",
				green2: "#00df7b",
				cream: "#f3efe0",
				cream2: "#eee8d2",
				cream3: "#e3d9b6",
			},
		},
	},
	plugins: [],
};
