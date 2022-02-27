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
				blue: "#1c5d99",
				blue2: "#257bcb",
				red: "#ff8966",
				green: "#00b865",
				cream: "#f3efe0",
				cream2: "#eee8d2",
				black: "#353535",
			},
		},
	},
	plugins: [],
};
