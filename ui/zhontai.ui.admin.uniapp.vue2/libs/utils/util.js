import test from 'uview-ui/libs/function/test.js'
import ClipboardJS from "uview-ui/components/u-tooltip/clipboard.min.js"
	
// 身份证脱敏
export function hideIdentity(identity) {
	if (identity == null || identity == undefined ) {
		return ''
	} else {
		return identity.replace(/^(.{1})(?:\w+)(.{1})$/, "\$1****************\$2");
	}
}

// 使用uni消息提示框
export const showToast = (msg, complete, duration = 1500) => {
	if(msg && duration > 0){
		uni.showToast({
			title: msg,
			mask: false,
			icon: 'none',
			duration: duration
		})
		
		if(complete){
			uni.$u.sleep(duration).then(() => {
				complete && complete()
			})
		}
	}
}

export const fileSizeFormat = (value) => {
    if(!value){
        return "0 KB"
    }
	// if(value < 1024){
	// 	return "1 KB"
	// }
    const unitArr = new Array("字节","KB","MB","GB","TB","PB","EB","ZB","YB")
    let index = 0
    const srcsize = parseFloat(value)
    index = Math.floor(Math.log(srcsize)/Math.log(1024))
    let size = srcsize/Math.pow(1024,index)
    size = Math.ceil(size).toFixed(0)
    return `${size} ${unitArr[index]}`
}

export const toHttps = (value) => {
    if(value){
        return value.replace('http:','https:')
    }
	return ''
}

const iPad = uni.getSystemInfoSync()?.model === 'iPad'
export function getPx(value, unit = false) {
	if (test.number(value)) {
		return unit ? `${value}px` : Number(value)
	}
	// 如果带有rpx，先取出其数值部分，再转为px值
	if (/(rpx|upx)$/.test(value)) {
		//修复平板显示过大的问题
		if(iPad){
			return unit ? `${parseInt(value)/2}px` : Number(parseInt(value)/2)
		}
		return unit ? `${uni.upx2px(parseInt(value))}px` : Number(uni.upx2px(parseInt(value)))
	}
	return unit ? `${parseInt(value)}px` : parseInt(value)
}

// 复制文本到粘贴板
export function setClipboardData(copyText, showToast = true, complete = null) {
	// #ifndef H5
	uni.setClipboardData({
		// 优先使用copyText字段，如果没有，则默认使用text字段当做复制的内容
		data: copyText,
		success: () => {
			showToast && uni.$u.toast('复制成功')
		},
		fail: () => {
			showToast && uni.$u.toast('复制失败')
		},
		complete: () => {
			complete && complete()
		}
	})
	// #endif

	// #ifdef H5
	let event = window.event || e || {}
	let clipboard = new ClipboardJS('', {
		text: () => copyText
	})
	clipboard.on('success', (e) => {
		showToast && uni.$u.toast('复制成功')
		clipboard.off('success')
		clipboard.off('error')
		// 在单页应用中，需要销毁DOM的监听
		clipboard.destroy()
		
		complete && complete()
	})
	clipboard.on('error', (e) => {
		showToast && uni.$u.toast('复制失败')
		clipboard.off('success')
		clipboard.off('error')
		// 在单页应用中，需要销毁DOM的监听
		clipboard.destroy()
	})
	clipboard.onClick(event)
	// #endif
}

export default {
	hideIdentity,
	showToast,
	fileSizeFormat,
	toHttps,
	getPx
}