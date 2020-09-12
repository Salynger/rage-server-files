Vue.prototype.$sendToClient = (event, data) => {
    const json = JSON.stringify(data);
    mp.trigger(event, json);
}
Vue.prototype.$parseEvent = (json) => {
    return JSON.parse(json);
}
Vue.prototype.$randomInt = (min, max) => {
    let rand = min - 0.5 + Math.random() * (max - min + 1);
    return Math.round(rand);
}

Vue.component('v-input', {
    inheritAttrs: false,
    template: `
<div class="input-box">
<input v-bind="$attrs" class="input" :value="value" @input="$emit('input', $event.target.value)" @change="$emit('change', $event.target.value)">
<span class="input--icon"><img :src="icon" class="input--icon-img"></span>
<div class="input--progress"><div class="input--progress-line" :style="{width: progress}"></div></div>
</div>`,
    props: {
        value: [String, Number],
        icon: {
            type: String,
            required: true
        },
        progressLine: {
            type: Number,
            default: 32
        }
    },
    computed: {
        progress() {
            return `${this.value}`.length / this.progressLine * 100 + '%'
        }
    }
})
Vue.component('v-input-checkbox', {
    inheritAttrs: false,
    template: `
<div class="input-box input-checkbox">
<p class="input akrobat-light" @click="disabled || $emit('input', !value)"><slot></slot></p>
<span class="input--icon"><img :src="icon" class="input--icon-img"></span>
<div class="input--progress"><div class="input--progress-line" :style="{width: value? '100%': 0}"></div></div>
</div>`,
    props: {
        value: Boolean,
        disabled: Boolean,
        icon: {
            type: String,
            required: true
        }
    }
})

Vue.component('v-input-range', {
    inheritAttrs: false,
    template: `<input type='range' class="input-range" :id="id" v-bind="$attrs" :value="value" :min="min" :step="step" :max="max" @input="$emit('input', $event.target.value)">`,
    data: () => ({
        style: null
    }),
    created() {
        this.style = document.createElement('style')
        document.head.appendChild(this.style)
        this.updateStyle()
    },
    destroyed() {
        document.head.removeChild(this.style)
        this.style = null
    },
    props: {
        value: [Number, String],
        min: [Number, String],
        max: [Number, String],
        step: [Number, String]
    },
    computed: {
        id() { return `ir-${this._uid}` }
    },
    methods: {
        updateStyle() {
            const min = this.min || 0,
                  max = this.max || this.$el.max,
                  v = this.value,
                  perc = (max) ? ~~(100*(v - min)/(max - min)) : v;
            this.style.textContent = `#${this.id}.input-range::-webkit-slider-runnable-track{background-size: ${perc}% 100%, 100% 100%; }`
        }
    },
    watch: {
        value() { this.updateStyle() }
    }
})
Vue.component('v-select', {
    inheritAttrs: false,
    template: `
<div class="input-box">
<select v-bind="$attrs" class="input" :value="value" :class="{'akrobat-light': !value}" @input="$emit('input', $event.target.value)" @change="$emit('change', $event.target.value)">
<option disabled :value="null">{{placeholder}}</option>
<option v-for="(o, i) in values" :key="i" :value="o.value">{{o.text}}</option>
</select>
<span class="input--icon"><img :src="icon" class="input--icon-img"></span>
<div class="input--progress"><div class="input--progress-line" :style="{width: value === null? 0: '100%'}"></div></div>
</div>`,
    props: {
        values: Array,
        value: [String, Number],
        placeholder: String,
        icon: {
            type: String,
            required: true
        },
        progressLine: {
            type: Number,
            default: 32
        }
    }
})