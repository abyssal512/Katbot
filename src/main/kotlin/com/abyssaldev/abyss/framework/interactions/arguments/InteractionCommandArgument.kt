package com.abyssaldev.abyss.framework.interactions.arguments

import com.abyssaldev.abyss.util.JsonHashable
import com.fasterxml.jackson.annotation.JsonProperty
import java.util.*

class InteractionCommandArgument(
    override val name: String,
    override val description: String,
    override val type: InteractionCommandArgumentType,
    val choices: Array<InteractionCommandArgumentChoice> = emptyArray(),
    @JsonProperty("required")
    val isRequired: Boolean = true
) : InteractionCommandOption, JsonHashable {
    override fun toJsonMap(): HashMap<String, Any> = hashMapOf(
        "name" to name,
        "description" to description,
        "type" to type.raw,
        "required" to isRequired,
        "choices" to choices
    )
}