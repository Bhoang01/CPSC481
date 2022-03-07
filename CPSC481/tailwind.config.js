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
				red2: "#c82e00",
				green: "#00b865",
				green2: "#00df7b",
				cream: "#f3efe0",
				cream2: "#eee8d2",
				cream3: "#e3d9b6",
				black: "#353535",
			},
		},
	},
	plugins: [],
};
